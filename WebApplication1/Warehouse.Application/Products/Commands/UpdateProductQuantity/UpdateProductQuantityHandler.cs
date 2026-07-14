using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UpdateProductQuantity;

public class UpdateProductQuantityHandler
    : IRequestHandler<UpdateProductQuantityCommand, UpdateProductQuantityResponse>
{
    private readonly IProductRepository _repository;

    public UpdateProductQuantityHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateProductQuantityResponse> Handle(
        UpdateProductQuantityCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId,cancellationToken
        );


        if (product == null)
            return new UpdateProductQuantityResponse(false);

        product.UpdateQuantity(
            request.QuantityInStock
        );


        await _repository.Update(product,cancellationToken);


        return new UpdateProductQuantityResponse(true);
    }
}