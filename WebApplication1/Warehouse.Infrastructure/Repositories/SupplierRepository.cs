using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly List<Supplier> suppliers = new();



    public Task Add(Supplier supplier)
    {
        suppliers.Add(supplier);

        return Task.CompletedTask;
    }



    public Task<Supplier?> GetById(Guid id)
    {
        var supplier = suppliers
            .FirstOrDefault(s => s.Id == id);

        return Task.FromResult(supplier);
    }



    public Task<IEnumerable<Supplier>> GetAll()
    {
        return Task.FromResult(
            suppliers.AsEnumerable()
        );
    }



    public Task Update(Supplier supplier)
    {
        return Task.CompletedTask;
    }



    public Task Delete(Guid id)
    {
        var supplier = suppliers
            .FirstOrDefault(s => s.Id == id);


        if (supplier != null)
        {
            supplier.Deactivate();
        }


        return Task.CompletedTask;
    }
}