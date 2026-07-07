using MediatR;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Handlers;


public class GetSupplierByIdHandler
    :IRequestHandler<GetSupplierByIdQuery,object?>
{

    private readonly ISupplierRepository _repository;


    public GetSupplierByIdHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }



    public async Task<object?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {

        return await _repository.GetById(
            request.Id
        );

    }
}