using MediatR;

namespace Warehouse.Application.Products.Commands;

public record UpdateProductPriceCommand(
    Guid ProductId,
    decimal Price
) : IRequest<bool>;