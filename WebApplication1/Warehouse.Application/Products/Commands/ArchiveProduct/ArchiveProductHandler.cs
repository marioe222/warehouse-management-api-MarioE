using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;
using Warehouse.Application.Common.Cache;

namespace Warehouse.Application.Products.Commands.ArchiveProduct;

public class ArchiveProductHandler
    : IRequestHandler<ArchiveProductCommand, ArchiveProductResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;

    public ArchiveProductHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<ArchiveProductResponse> Handle(
        ArchiveProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
            return new ArchiveProductResponse(false);


        product.Archive();


        await _repository.Update(
            product,
            cancellationToken
        );


        // Remove Redis cache because product data changed
        await _cache.RemoveAsync(
            CacheKeys.Product(request.ProductId),
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        return new ArchiveProductResponse(true);
    }
}