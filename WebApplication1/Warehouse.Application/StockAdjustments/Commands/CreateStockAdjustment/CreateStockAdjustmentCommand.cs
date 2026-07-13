using MediatR;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentCommand 
    : IRequest<CreateStockAdjustmentResponse>
{
    public Guid ProductId { get; set; }

    public int QuantityChange { get; set; }

    public string Reason { get; set; } = string.Empty;
}