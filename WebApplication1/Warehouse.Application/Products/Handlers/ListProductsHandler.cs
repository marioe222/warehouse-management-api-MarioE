using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class ListProductsHandler
    : IRequestHandler<ListProductsQuery, IEnumerable<object>>
{

    private readonly IProductRepository _repository;


    public ListProductsHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<IEnumerable<object>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {

        var products =
            await _repository.GetAll();


        if(request.OnlyAvailable)
        {
            products = products
                .Where(p => p.Quantity > 0);
        }


        return products;
    }
}