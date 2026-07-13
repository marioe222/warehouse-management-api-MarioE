using MediatR;

namespace Warehouse.Application.Products.Commands.AssignSupplier;

public record AssignSupplierCommand(
    Guid ProductId,
    Guid SupplierId
) : IRequest<AssignSupplierResponse>;