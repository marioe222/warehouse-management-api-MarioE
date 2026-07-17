using MediatR;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentHandler
    : IRequestHandler<CreateStockAdjustmentCommand, CreateStockAdjustmentResponse>
{
    private readonly IStockAdjustmentRepository _repository;


    public CreateStockAdjustmentHandler(
        IStockAdjustmentRepository repository)
    {
        _repository = repository;
    }


    public async Task<CreateStockAdjustmentResponse> Handle(
        CreateStockAdjustmentCommand request,
        CancellationToken cancellationToken)
    {
        var adjustment = new StockAdjustment
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            QuantityChange = request.QuantityChange,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };


        var createdAdjustment = await _repository.Add(
            adjustment,
            cancellationToken
        );


        return new CreateStockAdjustmentResponse
        {
            Id = createdAdjustment.Id,
            ProductId = createdAdjustment.ProductId,
            QuantityChange = createdAdjustment.QuantityChange,
            Reason = createdAdjustment.Reason,
            CreatedAt = createdAdjustment.CreatedAt
        };
    }
}