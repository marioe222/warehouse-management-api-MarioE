using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Interface;
using AutoMapper;
using Warehouse.Application.ViewModels;

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
            await _repository.GetAll();


        if (request.OnlyAvailable)
        {
            products = products
                .Where(p => p.QuantityInStock > 0);
        }


        var productViewModels =
            _mapper.Map<List<ProductViewModel>>(products);

        return new ListProductsResponse(productViewModels);
    }
}