using MediatR;

namespace Warehouse.Application.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQuery(
    Guid Id
) : IRequest<GetSupplierByIdResponse?>;