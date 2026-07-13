using MediatR;

namespace Warehouse.Application.Suppliers.Commands.DeactivateSupplier;

public record DeactivateSupplierCommand(
    Guid SupplierId
) : IRequest<DeactivateSupplierResponse>;