using Warehouse.Application.ViewModels;

namespace Warehouse.Application.Products.Queries.ListProducts;

public record ListProductsResponse(
    List<ProductViewModel> Products
);