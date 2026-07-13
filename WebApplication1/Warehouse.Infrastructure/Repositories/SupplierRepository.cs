using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbContext _context;

    public SupplierRepository(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task Add(
        Supplier supplier,
        CancellationToken cancellationToken)
    {
        await _context.Suppliers.AddAsync(
            supplier,
            cancellationToken
        );

        await _context.SaveChangesAsync(
            cancellationToken
        );
    }


    public async Task<Supplier?> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.Id == id,
                cancellationToken
            );
    }


    public async Task<IEnumerable<Supplier>> GetAll(
        CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .ToListAsync(cancellationToken);
    }


    public async Task Update(
        Supplier supplier,
        CancellationToken cancellationToken)
    {
        _context.Suppliers.Update(supplier);

        await _context.SaveChangesAsync(
            cancellationToken
        );
    }
}