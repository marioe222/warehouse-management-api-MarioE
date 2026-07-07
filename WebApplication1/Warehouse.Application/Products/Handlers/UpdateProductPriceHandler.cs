using MediatR;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class UpdateProductPriceHandler
    : IRequestHandler<UpdateProductPriceCommand, bool>
{

    private readonly IProductRepository _repository;


    public UpdateProductPriceHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<bool> Handle(
        UpdateProductPriceCommand request,
        CancellationToken cancellationToken)
    {

        var product = await _repository.GetById(
            request.ProductId
        );


        if(product == null)
            return false;



        product.UpdatePrice(
            request.Price
        );


        await _repository.Update(product);


        return true;
    }
}