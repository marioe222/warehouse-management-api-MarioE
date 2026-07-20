using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface ISupplierRepository
{
    Task Add(
        Supplier supplier,
        CancellationToken cancellationToken = default);

    Task<Supplier?> GetById(
        Guid id,
        CancellationToken cancellationToken);

    Task<List<Supplier>> GetAll(
        CancellationToken cancellationToken);

    Task Update(
        Supplier supplier,
        CancellationToken cancellationToken);
}