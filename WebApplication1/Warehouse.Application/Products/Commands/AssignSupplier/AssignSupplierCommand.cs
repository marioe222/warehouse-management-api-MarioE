using MediatR;

namespace Warehouse.Application.Products.Commands.AssignSupplier;

public record AssignSupplierCommand(
    int ProductId,
    int SupplierId
) : IRequest<AssignSupplierResponse>;