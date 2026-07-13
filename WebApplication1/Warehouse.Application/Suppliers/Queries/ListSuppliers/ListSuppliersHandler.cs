using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Queries.ListSuppliers;

public class ListSuppliersHandler
    : IRequestHandler<ListSuppliersQuery, ListSuppliersResponse>
{
    private readonly ISupplierRepository _repository;


    public ListSuppliersHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }


    public async Task<ListSuppliersResponse> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = await _repository.GetAll(
            cancellationToken
        );


        return new ListSuppliersResponse(
            suppliers
        );
    }
}