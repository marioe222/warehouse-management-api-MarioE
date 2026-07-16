namespace Warehouse.Application.Products.Queries.GetProductById;

public record GetProductByIdResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    bool IsArchived,
    Guid? SupplierId,
    string? SupplierName
);