using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbContext _context;

    public ProductRepository(WarehouseDbContext context)
    {
        _context = context;
    }


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