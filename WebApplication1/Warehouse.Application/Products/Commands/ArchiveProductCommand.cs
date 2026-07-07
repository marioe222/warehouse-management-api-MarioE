using MediatR;

namespace Warehouse.Application.Products.Commands;

public record ArchiveProductCommand(
    Guid ProductId
) : IRequest<bool>;