using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Commands.DeactivateSupplier;

public class DeactivateSupplierHandler
    : IRequestHandler<DeactivateSupplierCommand, DeactivateSupplierResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ISupplierRepository _repository;

    public DeactivateSupplierHandler(
        ISupplierRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<DeactivateSupplierResponse> Handle(
        DeactivateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetById(
            request.SupplierId,
            cancellationToken
        );


        if (supplier == null)
            return new DeactivateSupplierResponse(false);


        supplier.Deactivate();


        await _repository.Update(
            supplier,
            cancellationToken
        );


        await _cache.RemoveAsync(
            $"supplier:{request.SupplierId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "suppliers",
            cancellationToken);


        return new DeactivateSupplierResponse(true);
    }
}