using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.AssignSupplier;

public class AssignSupplierHandler
    : IRequestHandler<AssignSupplierCommand, AssignSupplierResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public AssignSupplierHandler(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<AssignSupplierResponse> Handle(
        AssignSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetById(
            request.ProductId
        );

        if (product == null)
            return new AssignSupplierResponse(false);


        var supplier = await _supplierRepository.GetById(
            request.SupplierId
        );

        if (supplier == null)
            return new AssignSupplierResponse(false);


        if (!supplier.IsActive)
            return new AssignSupplierResponse(false);


        product.AssignSupplier(
            supplier.Id,
            supplier.Name
        );


        await _productRepository.Update(product);


        return new AssignSupplierResponse(true);
    }
}