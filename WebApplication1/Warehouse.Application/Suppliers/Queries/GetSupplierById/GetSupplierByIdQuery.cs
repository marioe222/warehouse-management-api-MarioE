using MediatR;

namespace Warehouse.Application.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQuery(
    int Id
) : IRequest<GetSupplierByIdResponse?>;