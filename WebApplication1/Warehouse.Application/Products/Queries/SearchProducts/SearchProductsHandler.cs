using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Interface;
using Warehouse.Application.Common.Cache;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public SearchProductsHandler(
        IProductRepository repository,
        IMapper mapper,
        IDistributedCache cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<SearchProductsResponse> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.SearchProducts(
            request.Name,
            request.Supplier);

        var cacheValue = await _cache.GetStringAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue == null)
        {
            var products = await _repository.GetAll(
                cancellationToken);

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

            var response =
                new SearchProductsResponse(productViewModels);

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

        return JsonSerializer.Deserialize<SearchProductsResponse>(
            cacheValue)!;
    }
}