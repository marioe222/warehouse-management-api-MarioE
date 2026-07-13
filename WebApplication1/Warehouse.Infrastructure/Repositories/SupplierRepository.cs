using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    public Task Add(Supplier supplier)
    {
        FakeWarehouseStore.Suppliers.Add(supplier);

        return Task.CompletedTask;
    }


    public Task<Supplier?> GetById(Guid id)
    {
        var supplier = FakeWarehouseStore.Suppliers
            .FirstOrDefault(s => s.Id == id);

        return Task.FromResult(supplier);
    }


    public Task<IEnumerable<Supplier>> GetAll()
    {
        IEnumerable<Supplier> suppliers =
            FakeWarehouseStore.Suppliers;

        return Task.FromResult(suppliers);
    }


    public Task Update(Supplier supplier)
    {
        return Task.CompletedTask;
    }
}