using Warehouse.Domain.Entities;

namespace Warehouse.Application.Suppliers.Queries.ListSuppliers;

public record ListSuppliersResponse(
    IEnumerable<Supplier> Suppliers
);