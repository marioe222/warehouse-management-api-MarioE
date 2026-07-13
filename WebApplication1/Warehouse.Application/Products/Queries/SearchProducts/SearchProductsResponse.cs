namespace Warehouse.Application.Products.Queries.SearchProducts;

public record SearchProductsResponse(
    IEnumerable<object> Products
);