using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly List<Product> _products = new();



    public Task Add(Product product)
    {
        _products.Add(product);

        return Task.CompletedTask;
    }



    public Task<Product?> GetById(Guid id)
    {
        var product = _products
            .FirstOrDefault(p => p.Id == id);

        return Task.FromResult(product);
    }



    public Task<IEnumerable<Product>> GetAll()
    {
        return Task.FromResult(
            _products.AsEnumerable()
        );
    }



    public Task Update(Product product)
    {
        var existing = _products
            .FirstOrDefault(p => p.Id == product.Id);


        if(existing != null)
        {
            var index = _products.IndexOf(existing);

            _products[index] = product;
        }


        return Task.CompletedTask;
    }



    public Task Delete(Product product)
    {
        _products.Remove(product);

        return Task.CompletedTask;
    }

}