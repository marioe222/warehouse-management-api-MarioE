using MediatR;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class AssignSupplierHandler
    : IRequestHandler<AssignSupplierCommand, bool>
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




    public async Task<bool> Handle(
        AssignSupplierCommand request,
        CancellationToken cancellationToken)
    {

        var product =
            await _productRepository.GetById(
                request.ProductId
            );


        if(product == null)
            return false;



        var supplier =
            await _supplierRepository.GetById(
                request.SupplierId
            );


        if(supplier == null)
            return false;



        if(!supplier.IsActive)
            return false;



        // We will add this method in Product entity
        product.AssignSupplier(
            supplier
        );


        await _productRepository.Update(product);



        return true;
    }
}