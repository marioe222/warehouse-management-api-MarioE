using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UpdateProductQuantity;

public class UpdateProductQuantityHandler
    : IRequestHandler<UpdateProductQuantityCommand, UpdateProductQuantityResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;

    public UpdateProductQuantityHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<UpdateProductQuantityResponse> Handle(
        UpdateProductQuantityCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
            return new UpdateProductQuantityResponse(false);


        product.UpdateQuantity(
            request.QuantityInStock
        );


        await _repository.Update(
            product,
            cancellationToken
        );


        // Remove Redis cache because product quantity changed
        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        return new UpdateProductQuantityResponse(true);
    }
}