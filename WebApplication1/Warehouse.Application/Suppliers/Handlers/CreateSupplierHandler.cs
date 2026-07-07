using MediatR;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Handlers;


public class CreateSupplierHandler
    :IRequestHandler<CreateSupplierCommand, Guid>
{

    private readonly ISupplierRepository _repository;


    public CreateSupplierHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }



    public async Task<Guid> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {

        var supplier =
            new Supplier(
                request.Name,
                request.ContactEmail
            );


        await _repository.Add(supplier);


        return supplier.Id;
    }
}