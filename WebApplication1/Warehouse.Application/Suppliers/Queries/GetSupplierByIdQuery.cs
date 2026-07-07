using MediatR;

namespace Warehouse.Application.Suppliers.Queries;


public record GetSupplierByIdQuery(
    Guid Id
)
    :IRequest<object?>;