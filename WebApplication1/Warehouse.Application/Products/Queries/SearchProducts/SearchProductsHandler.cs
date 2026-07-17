using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public SearchProductsHandler(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SearchProductsResponse> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _repository.GetAll(
            cancellationToken
        );


        var query = products
            .Where(p => !p.IsArchived);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(p =>
                p.Name.Contains(
                    request.Name,
                    StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Supplier))
            query = query.Where(p =>
                p.SupplierName != null &&
                p.SupplierName.Contains(
                    request.Supplier,
                    StringComparison.OrdinalIgnoreCase));

        var productViewModels =
            _mapper.Map<List<ProductViewModel>>(query.ToList());

        return new SearchProductsResponse(productViewModels);
    }
}