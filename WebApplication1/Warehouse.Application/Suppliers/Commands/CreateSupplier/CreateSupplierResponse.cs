namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public record CreateSupplierResponse(
    Guid SupplierId,
    string Name,
    string ContactEmail
);