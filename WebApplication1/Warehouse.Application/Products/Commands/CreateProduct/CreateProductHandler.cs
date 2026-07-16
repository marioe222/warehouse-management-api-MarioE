using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.CreateProduct;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _repository;
    private readonly IDistributedCache _cache;

    public CreateProductHandler(
        IProductRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<CreateProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var expiryDateUtc = DateTime.SpecifyKind(
            request.ExpiryDate,
            DateTimeKind.Utc
        );

        var product = new Product(
            request.Name,
            request.Sku,
            request.Description,
            request.Price,
            request.QuantityInStock,
            request.SupplierName,
            expiryDateUtc
        );

        await _repository.Add(
            product,
            cancellationToken
        );

        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);

        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);

        return new CreateProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.QuantityInStock,
            product.IsArchived,
            product.SupplierId,
            product.SupplierName
        );
    }
}