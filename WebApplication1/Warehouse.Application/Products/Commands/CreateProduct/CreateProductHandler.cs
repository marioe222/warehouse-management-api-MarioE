using MediatR;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.CreateProduct;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _repository;

    public CreateProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Sku,
            request.Description,
            request.Price,
            request.QuantityInStock,
            request.SupplierName,
            request.ExpiryDate
        );


        await _repository.Add(
            product,
            cancellationToken
        );


        return new CreateProductResponse(product.Id);
    }
}