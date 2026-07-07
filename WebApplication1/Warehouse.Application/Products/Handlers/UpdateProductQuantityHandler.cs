using MediatR;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class UpdateProductQuantityHandler
    : IRequestHandler<UpdateProductQuantityCommand, bool>
{

    private readonly IProductRepository _repository;


    public UpdateProductQuantityHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<bool> Handle(
        UpdateProductQuantityCommand request,
        CancellationToken cancellationToken)
    {

        var product = await _repository.GetById(
            request.ProductId
        );


        if(product == null)
            return false;


        product.UpdateQuantity(
            request.QuantityInStock
        );


        await _repository.Update(product);


        return true;
    }
}