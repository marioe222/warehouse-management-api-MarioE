using MediatR;

namespace Warehouse.Application.Products.Commands;

public record UpdateProductQuantityCommand(
    Guid ProductId,
    int QuantityInStock
) : IRequest<bool>;