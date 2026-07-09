using MediatR;

namespace Warehouse.Application.Products.Commands.ArchiveProduct;

public record ArchiveProductCommand(
    int ProductId
) : IRequest<ArchiveProductResponse>;