using MediatR;

namespace Warehouse.Application.Products.Commands;

public record AssignSupplierCommand(
    Guid ProductId,
    Guid SupplierId
) : IRequest<bool>;