using MediatR;

namespace Warehouse.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int QuantityInStock,
    string SupplierName,
    int SupplierId,
    DateTime? ExpiryDate
) : IRequest<CreateProductResponse>;