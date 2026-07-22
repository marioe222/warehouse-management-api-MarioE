using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public class UploadProductImageHandler
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;

    public UploadProductImageHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<UploadProductImageResponse> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null) return new UploadProductImageResponse(false);


        // TODO:
        // Add image upload logic here
        // Update product image property
        // await _repository.Update(product, cancellationToken);


        // Remove Redis cache because product data changed
        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        return new UploadProductImageResponse(true);
    }
}