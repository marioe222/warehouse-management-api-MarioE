using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdHandler
    : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdResponse?>
{
    private readonly ISupplierRepository _repository;


    public GetSupplierByIdHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }


    public async Task<GetSupplierByIdResponse?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetById(
            request.Id,
            cancellationToken
        );


        if (supplier == null)
            return null;


        return new GetSupplierByIdResponse(
            supplier
        );
    }
}