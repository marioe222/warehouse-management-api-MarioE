using MediatR;
using Warehouse.Domain.Interface;
using AutoMapper;
using Warehouse.Application.ViewModels;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;


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
                p.SupplierName != null &&
                p.SupplierName.Contains(
                    request.Supplier,
                    StringComparison.OrdinalIgnoreCase
                ));
        }


        var productViewModels =
            _mapper.Map<List<ProductViewModel>>(query.ToList());

        return new SearchProductsResponse(
            productViewModels
        );  
    }
}