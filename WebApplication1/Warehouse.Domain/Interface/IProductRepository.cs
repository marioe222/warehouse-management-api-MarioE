using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IProductRepository
{
    Task Add(
        Product product,
        CancellationToken cancellationToken);

    Task<Product?> GetById(
        Guid id,
        CancellationToken cancellationToken);

    Task<List<Product>> GetAll(
        CancellationToken cancellationToken);

    Task<List<Product>> GetExpiringProducts(
        DateOnly date,
        CancellationToken cancellationToken);

    Task Update(
        Product product,
        CancellationToken cancellationToken);
}