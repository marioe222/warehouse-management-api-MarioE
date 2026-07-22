using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.AssignSupplier;

public class AssignSupplierHandler
    : IRequestHandler<AssignSupplierCommand, AssignSupplierResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public AssignSupplierHandler(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository,
        IDistributedCache cache)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _cache = cache;
    }

    public async Task<AssignSupplierResponse> Handle(
        AssignSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetById(
            request.ProductId,
            cancellationToken
        );

        if (product == null)
            return new AssignSupplierResponse(false);


        var supplier = await _supplierRepository.GetById(
            request.SupplierId,
            cancellationToken
        );

        if (supplier == null)
            return new AssignSupplierResponse(false);


        if (!supplier.IsActive)
            return new AssignSupplierResponse(false);


        product.AssignSupplier(supplier);


        await _productRepository.Update(
            product,
            cancellationToken
        );


        // Remove Redis cache because product changed
        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        return new AssignSupplierResponse(true);
    }
}