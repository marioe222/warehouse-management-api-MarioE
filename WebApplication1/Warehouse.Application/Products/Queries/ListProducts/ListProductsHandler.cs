using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.ListProducts;

public class ListProductsHandler
    : IRequestHandler<ListProductsQuery, ListProductsResponse>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public ListProductsHandler(
        IProductRepository repository,
        IMapper mapper,
        IDistributedCache cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ListProductsResponse> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"products:{request.OnlyAvailable}";

        var cacheValue = await _cache.GetStringAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue == null)
        {
            var products = await _repository.GetAll(
                cancellationToken);

            if (request.OnlyAvailable)
            {
                products = products
                    .Where(p => p.QuantityInStock > 0)
                    .ToList();
            }

            var productViewModels =
                _mapper.Map<List<ProductViewModel>>(products);

            var response =
                new ListProductsResponse(productViewModels);

            var serializedResponse =
                JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(
                cacheKey,
                serializedResponse,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                },
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<ListProductsResponse>(
            cacheValue)!;
    }
}