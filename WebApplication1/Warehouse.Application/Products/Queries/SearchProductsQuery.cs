using MediatR;

namespace Warehouse.Application.Products.Queries;

public record SearchProductsQuery(
    string? Name,
    string? Supplier
) : IRequest<IEnumerable<object>>;