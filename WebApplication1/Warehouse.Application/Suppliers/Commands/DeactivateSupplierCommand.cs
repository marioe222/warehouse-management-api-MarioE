using MediatR;

namespace Warehouse.Application.Suppliers.Commands;


public record DeactivateSupplierCommand(
    Guid SupplierId
) : IRequest<bool>;