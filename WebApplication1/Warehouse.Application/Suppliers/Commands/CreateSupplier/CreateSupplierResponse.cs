namespace Warehouse.Application.Suppliers.Commands.CreateSupplier;

public record CreateSupplierResponse(
    Guid Id,
    string Name,
    string ContactEmail
);