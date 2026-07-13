using MediatR;

namespace Warehouse.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(
    Guid Id
) : IRequest<GetProductByIdResponse?>;