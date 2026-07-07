using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface ISupplierRepository
{
    Task<Supplier?> GetById(Guid id);

    Task<IEnumerable<Supplier>> GetAll();

    Task Add(Supplier supplier);

    Task Update(Supplier supplier);

    Task Delete(Guid id);
}