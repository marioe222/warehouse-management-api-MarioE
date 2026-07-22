using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UpdateProductPrice;

public class UpdateProductPriceHandler
    : IRequestHandler<UpdateProductPriceCommand, UpdateProductPriceResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;

    public UpdateProductPriceHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<UpdateProductPriceResponse> Handle(
        UpdateProductPriceCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
            return new UpdateProductPriceResponse(false);


        product.UpdatePrice(
            request.Price
        );


        await _repository.Update(
            product,
            cancellationToken
        );


        // Remove Redis cache because product price changed
        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        return new UpdateProductPriceResponse(true);
    }
}