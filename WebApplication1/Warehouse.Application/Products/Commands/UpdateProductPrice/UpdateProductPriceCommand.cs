using MediatR;

namespace Warehouse.Application.Products.Commands.UpdateProductPrice;

public record UpdateProductPriceCommand(
    Guid ProductId,
    decimal Price
) : IRequest<UpdateProductPriceResponse>;