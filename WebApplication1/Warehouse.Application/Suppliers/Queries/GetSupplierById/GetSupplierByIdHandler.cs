using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdHandler
    : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdResponse?>
{
    private readonly IDistributedCache _cache;
    private readonly ISupplierRepository _repository;

    public GetSupplierByIdHandler(
        ISupplierRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<GetSupplierByIdResponse?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"supplier:{request.Id}";

        var cacheValue = await _cache.GetStringAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue == null)
        {
            var supplier = await _repository.GetById(
                request.Id,
                cancellationToken);

            if (supplier == null) return null;

            var response = new GetSupplierByIdResponse(
                supplier);

            var serializedSupplier =
                JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(
                cacheKey,
                serializedSupplier,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                },
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<GetSupplierByIdResponse>(
            cacheValue);
    }
}