using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.ListProducts;

public class ListProductsHandler
    : IRequestHandler<ListProductsQuery, ListProductsResponse>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ListProductsHandler(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ListProductsResponse> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products =
            await _repository.GetAll(cancellationToken);

        if (request.OnlyAvailable)
        {
            products = products
                .Where(p => p.QuantityInStock > 0)
                .ToList();
        }

        var productViewModels =
            _mapper.Map<List<ProductViewModel>>(products);

        return new ListProductsResponse(productViewModels);
    }
}