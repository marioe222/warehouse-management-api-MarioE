using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.ListProducts;

public class ListProductsHandler
    : IRequestHandler<ListProductsQuery, ListProductsResponse>
{
    private readonly IProductRepository _repository;


    public ListProductsHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }


    public async Task<ListProductsResponse> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products =
            await _repository.GetAll();


        if (request.OnlyAvailable)
        {
            products = products
                .Where(p => p.QuantityInStock > 0);
        }


        return new ListProductsResponse(products);
    }
}