using MediatR;

namespace Warehouse.Application.Products.Queries;

public record ListProductsQuery(
    bool OnlyAvailable
) : IRequest<IEnumerable<object>>;