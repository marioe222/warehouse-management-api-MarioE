using MediatR;

namespace Warehouse.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(
    int Id
) : IRequest<GetProductByIdResponse?>;