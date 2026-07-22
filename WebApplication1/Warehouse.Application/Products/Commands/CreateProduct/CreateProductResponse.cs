namespace Warehouse.Application.Products.Commands.CreateProduct;

public record CreateProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    bool IsArchived,
    Guid? SupplierId,
    string? SupplierName
);