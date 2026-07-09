using MediatR;

namespace Warehouse.Application.Products.Commands.ArchiveProduct;

public record ArchiveProductCommand(
    Guid ProductId
) : IRequest<ArchiveProductResponse>;