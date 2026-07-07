using MediatR;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Handlers;


public class ListSuppliersHandler
    :IRequestHandler<ListSuppliersQuery,IEnumerable<object>>
{

    private readonly ISupplierRepository _repository;


    public ListSuppliersHandler(
        ISupplierRepository repository)
    {
        _repository = repository;
    }



    public async Task<IEnumerable<object>> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {

        return await _repository.GetAll();

    }
}