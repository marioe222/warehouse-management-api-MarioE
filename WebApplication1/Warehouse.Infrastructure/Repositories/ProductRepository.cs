using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Repositories;

public interface IProductRepository
{
    Task Add(
        Product product,
        CancellationToken cancellationToken = default);

    Task<Product?> GetById(
        Guid id,
        CancellationToken cancellationToken = default);


    public async Task Add(
        Product product,
        CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken
        );

        await _context.SaveChangesAsync(
            cancellationToken
        );
    }


    public async Task<Product?> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken
            );
    }


    public async Task<IEnumerable<Product>> GetAll(
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .ToListAsync(cancellationToken);
    }


    public async Task Update(
        Product product,
        CancellationToken cancellationToken)
    {
        _context.Products.Update(product);

        await _context.SaveChangesAsync(
            cancellationToken
        );
    }
}