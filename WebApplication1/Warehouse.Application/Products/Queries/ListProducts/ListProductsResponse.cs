namespace Warehouse.Application.Products.Queries.ListProducts;

public record ListProductsResponse(
    IEnumerable<object> Products
);