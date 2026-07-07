using MediatR;

namespace Warehouse.Application.Suppliers.Commands;


public record CreateSupplierCommand(
    string Name,
    string Country,
    string ContactEmail,
    string PhoneNumber
) : IRequest<Guid>;