using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface ISupplierRepository
{
    Task Add(Supplier supplier);

    Task<Supplier?> GetById(int id);

    Task<IEnumerable<Supplier>> GetAll();

    Task Update(Supplier supplier);
}