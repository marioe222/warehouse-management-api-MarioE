using MediatR;

namespace Warehouse.Application.Products.Queries;

public record GetProductByIdQuery(
    Guid Id
) : IRequest<object?>;