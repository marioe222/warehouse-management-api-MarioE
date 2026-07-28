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
        Console.WriteLine(
            $"Assign supplier started. ProductId: {request.ProductId}, SupplierId: {request.SupplierId}"
        );


        var product = await _productRepository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
        {
            Console.WriteLine(
                $"PRODUCT NOT FOUND: {request.ProductId}"
            );

            return new AssignSupplierResponse(false);
        }


        Console.WriteLine(
            $"PRODUCT FOUND: {product.Id} - {product.Name}"
        );


        var supplier = await _supplierRepository.GetById(
            request.SupplierId,
            cancellationToken
        );


        if (supplier == null)
        {
            Console.WriteLine(
                $"SUPPLIER NOT FOUND: {request.SupplierId}"
            );

            return new AssignSupplierResponse(false);
        }


        Console.WriteLine(
            $"SUPPLIER FOUND: {supplier.Id} - {supplier.Name}"
        );


        if (!supplier.IsActive)
        {
            Console.WriteLine(
                $"SUPPLIER INACTIVE: {supplier.Id}"
            );

            return new AssignSupplierResponse(false);
        }


        product.AssignSupplier(supplier);


        await _productRepository.Update(
            product,
            cancellationToken
        );


        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        Console.WriteLine(
            $"Supplier {supplier.Id} assigned successfully to product {product.Id}"
        );


        return new AssignSupplierResponse(true);
    }
}