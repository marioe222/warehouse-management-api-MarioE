using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public class CreateSupplierHandler
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    private readonly ISupplierRepository _repository;
    private readonly IDistributedCache _cache;

    public CreateSupplierHandler(
        ISupplierRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<CreateSupplierResponse> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = new Supplier(
            request.Name,
            request.ContactEmail
        );


        await _repository.Add(
            supplier,
            cancellationToken
        );


        await _cache.RemoveAsync(
            "suppliers",
            cancellationToken);


        return new CreateSupplierResponse(
            supplier.Id,
            supplier.Name,
            supplier.ContactEmail
        );
    }
}