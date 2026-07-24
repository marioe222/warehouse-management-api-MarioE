using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Common.Cache;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse?>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<GetProductByIdResponse?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.Product(request.Id);

        var cacheValue = await _cache.GetStringAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue == null)
        {
            var product = await _repository.GetById(
                request.Id,
                cancellationToken);

            if (product == null)
                return null;

            var response = new GetProductByIdResponse(
                product.Id,
                product.Name,
                product.Price,
                product.QuantityInStock,
                product.IsArchived,
                product.SupplierId,
                product.Supplier != null
                    ? product.Supplier.Name
                    : product.SupplierName
            );

            var serializedProduct = JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(
                cacheKey,
                serializedProduct,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                },
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<GetProductByIdResponse>(
            cacheValue);
    }
}