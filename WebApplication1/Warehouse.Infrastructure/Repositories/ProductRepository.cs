using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    public Task Add(Product product)
    {
        FakeWarehouseStore.Products.Add(product);

        return Task.CompletedTask;
    }


    public Task<Product?> GetById(Guid id)
    {
        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        return Task.FromResult(product);
    }


    public Task<IEnumerable<Product>> GetAll()
    {
        return Task.FromResult<IEnumerable<Product>>(
            FakeWarehouseStore.Products
        );
    }  


    public async Task Update(Product product)
    {
        await Task.CompletedTask;
    }
}