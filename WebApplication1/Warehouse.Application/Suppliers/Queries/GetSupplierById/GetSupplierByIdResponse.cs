using Warehouse.Domain.Entities;

namespace Warehouse.Application.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdResponse(
    Supplier Supplier
);