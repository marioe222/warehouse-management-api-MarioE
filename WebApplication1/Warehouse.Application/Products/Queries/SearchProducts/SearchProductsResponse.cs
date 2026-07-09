using Warehouse.Domain.Entities;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public record SearchProductsResponse(
    IEnumerable<Product> Products
);