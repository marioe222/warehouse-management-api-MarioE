using MediatR;

namespace Warehouse.Application.Products.Commands.UpdateProductQuantity;

public record UpdateProductQuantityCommand(
    int ProductId,
    int QuantityInStock
) : IRequest<UpdateProductQuantityResponse>;