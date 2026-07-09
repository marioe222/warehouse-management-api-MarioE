using MediatR;

namespace Warehouse.Application.Products.Commands.UpdateProductPrice;

public record UpdateProductPriceCommand(
    int ProductId,
    decimal Price
) : IRequest<UpdateProductPriceResponse>;