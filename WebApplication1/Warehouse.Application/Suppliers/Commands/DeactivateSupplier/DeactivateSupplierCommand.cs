using MediatR;

namespace Warehouse.Application.Suppliers.Commands.DeactivateSupplier;

public record DeactivateSupplierCommand(
    int SupplierId
) : IRequest<DeactivateSupplierResponse>;