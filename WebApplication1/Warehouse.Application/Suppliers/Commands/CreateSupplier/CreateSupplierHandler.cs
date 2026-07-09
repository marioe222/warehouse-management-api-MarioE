using MediatR;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public class CreateSupplierHandler
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    private readonly ISupplierRepository _repository;


    public CreateSupplierHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }


    public async Task<CreateSupplierResponse> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier =
            new Supplier(
                request.Name,
                request.ContactEmail
            );


        await _repository.Add(supplier);


        return new CreateSupplierResponse(
            supplier.Id
        );
    }
}