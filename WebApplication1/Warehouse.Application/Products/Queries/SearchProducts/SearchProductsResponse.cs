using Warehouse.Application.ViewModels;

namespace Warehouse.Application.Products.Queries.SearchProducts;

public record SearchProductsResponse(
    List<ProductViewModel> Products
);