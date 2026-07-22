using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class StockAdjustmentRepository : IStockAdjustmentRepository
{
    private readonly WarehouseDbContext _context;


    public StockAdjustmentRepository(
        WarehouseDbContext context)
    {
        _context = context;
    }


    public async Task<StockAdjustment> Add(
        StockAdjustment stockAdjustment,
        CancellationToken cancellationToken)
    {
        await _context.StockAdjustments.AddAsync(
            stockAdjustment,
            cancellationToken
        );

        await _context.SaveChangesAsync(
            cancellationToken
        );

        return stockAdjustment;
    }


    public async Task<StockAdjustment?> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.StockAdjustments
            .FirstOrDefaultAsync(
                s => s.Id == id,
                cancellationToken
            );
    }


    public async Task<IEnumerable<StockAdjustment>> GetAll(
        CancellationToken cancellationToken)
    {
        return await _context.StockAdjustments
            .ToListAsync(
                cancellationToken
            );
    }
}