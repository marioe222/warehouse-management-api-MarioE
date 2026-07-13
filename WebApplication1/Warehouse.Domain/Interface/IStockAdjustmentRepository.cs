using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IStockAdjustmentRepository
{
    Task<StockAdjustment> Add(
        StockAdjustment stockAdjustment,
        CancellationToken cancellationToken);

    Task<StockAdjustment?> GetById(
        Guid id,
        CancellationToken cancellationToken);

    Task<IEnumerable<StockAdjustment>> GetAll(
        CancellationToken cancellationToken);
}