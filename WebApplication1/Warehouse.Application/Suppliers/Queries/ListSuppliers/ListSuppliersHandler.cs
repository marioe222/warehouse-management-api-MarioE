using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Queries.ListSuppliers;

public class ListSuppliersHandler
    : IRequestHandler<ListSuppliersQuery, ListSuppliersResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ISupplierRepository _repository;

    public ListSuppliersHandler(
        ISupplierRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<ListSuppliersResponse> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = "suppliers";

        var cacheValue = await _cache.GetStringAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue == null)
        {
            var suppliers = await _repository.GetAll(
                cancellationToken);

            var response = new ListSuppliersResponse(
                suppliers);

            var serializedSuppliers =
                JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(
                cacheKey,
                serializedSuppliers,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                },
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<ListSuppliersResponse>(
            cacheValue)!;
    }
}