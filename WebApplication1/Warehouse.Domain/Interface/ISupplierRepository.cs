using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface ISupplierRepository
{
    Task Add(
        Supplier supplier,
        CancellationToken cancellationToken = default);

    Task<Supplier?> GetById(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Supplier>> GetAll(
        CancellationToken cancellationToken = default);

    Task Update(
        Supplier supplier,
        CancellationToken cancellationToken = default);
}