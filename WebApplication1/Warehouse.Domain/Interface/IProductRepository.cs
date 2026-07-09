using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IProductRepository
{
    Task Add(Product product);

    Task<Product?> GetById(Guid id);

    Task<IEnumerable<Product>> GetAll();

    Task Update(Product product);
}