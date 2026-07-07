using MediatR;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Handlers;


public class DeactivateSupplierHandler
    :IRequestHandler<DeactivateSupplierCommand,bool>
{

    private readonly ISupplierRepository _repository;


    public DeactivateSupplierHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }



    public async Task<bool> Handle(
        DeactivateSupplierCommand request,
        CancellationToken cancellationToken)
    {

        var supplier =
            await _repository.GetById(
                request.SupplierId
            );


        if(supplier == null)
            return false;


        supplier.Deactivate();


        await _repository.Update(supplier);


        return true;
    }
}