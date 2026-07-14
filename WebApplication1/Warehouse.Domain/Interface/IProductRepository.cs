using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IProductRepository
{
    Task Add(
        Product product,
        CancellationToken cancellationToken = default);

    Task<Product?> GetById(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Product>> GetAll(
        CancellationToken cancellationToken = default);

    Task Update(
        Product product,
        CancellationToken cancellationToken = default);
}