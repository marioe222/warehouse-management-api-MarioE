using Warehouse.Domain.Entities;

namespace Warehouse.Application.Products.Queries.ListProducts;

public record ListProductsResponse(
    IEnumerable<Product> Products
);