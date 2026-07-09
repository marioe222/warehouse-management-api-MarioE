using MediatR;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UpdateProductPrice;

public class UpdateProductPriceHandler
    : IRequestHandler<UpdateProductPriceCommand, UpdateProductPriceResponse>
{
    private readonly IProductRepository _repository;


    public UpdateProductPriceHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }


    public async Task<UpdateProductPriceResponse> Handle(
        UpdateProductPriceCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId
        );


        if (product == null)
            return new UpdateProductPriceResponse(false);


        product.UpdatePrice(
            request.Price
        );


        await _repository.Update(product);


        return new UpdateProductPriceResponse(true);
    }
}