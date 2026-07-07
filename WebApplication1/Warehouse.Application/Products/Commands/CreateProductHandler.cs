using MediatR;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands;

public class CreateProductHandler 
    : IRequestHandler<CreateProductCommand, Guid>
{

    private readonly IProductRepository _repository;


    public CreateProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {

        var product = new Product(
            request.Name,
            request.Sku,
            request.Price,
            request.QuantityInStock
        );


        await _repository.Add(product);


        return product.Id;
    }
}