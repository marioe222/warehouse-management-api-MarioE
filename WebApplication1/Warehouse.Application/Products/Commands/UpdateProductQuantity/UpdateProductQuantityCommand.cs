using MediatR;

namespace Warehouse.Application.Products.Commands.UpdateProductQuantity;

public record UpdateProductQuantityCommand(
    Guid ProductId,
    int QuantityInStock
) : IRequest<UpdateProductQuantityResponse>;