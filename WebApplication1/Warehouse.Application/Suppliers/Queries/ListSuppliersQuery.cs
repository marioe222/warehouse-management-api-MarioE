using MediatR;

namespace Warehouse.Application.Suppliers.Queries;


public record ListSuppliersQuery()
    : IRequest<IEnumerable<object>>;