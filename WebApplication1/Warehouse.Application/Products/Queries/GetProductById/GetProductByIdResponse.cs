using Warehouse.Domain.Entities;

namespace Warehouse.Application.Products.Queries.GetProductById;

public record GetProductByIdResponse(
    Product? Product
);