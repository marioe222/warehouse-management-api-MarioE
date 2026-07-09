using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
{
    private readonly IProductRepository _repository;


    public SearchProductsHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }


    public async Task<SearchProductsResponse> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _repository.GetAll();


        var query = products
            .Where(p => !p.IsArchived);


        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p =>
                p.Name.Contains(
                    request.Name,
                    StringComparison.OrdinalIgnoreCase
                ));
        }


        if (!string.IsNullOrWhiteSpace(request.Supplier))
        {
            query = query.Where(p =>
                p.SupplierId != null);
        }


        return new SearchProductsResponse(
            query.ToList()
        );
    }
}