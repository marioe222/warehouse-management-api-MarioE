using MediatR;

namespace Warehouse.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int QuantityInStock,
    string? SupplierName,
    DateTime? ExpiryDate
) : IRequest<Guid>;