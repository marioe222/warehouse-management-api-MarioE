using MediatR;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Commands.DeactivateSupplier;

public class DeactivateSupplierHandler
    : IRequestHandler<DeactivateSupplierCommand, DeactivateSupplierResponse>
{
    private readonly ISupplierRepository _repository;

    public DeactivateSupplierHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
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


        return new DeactivateSupplierResponse(true);
    }
}