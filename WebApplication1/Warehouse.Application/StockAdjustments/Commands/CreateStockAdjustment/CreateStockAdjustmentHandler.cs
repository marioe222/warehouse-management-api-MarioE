using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentHandler
    : IRequestHandler<CreateStockAdjustmentCommand, CreateStockAdjustmentResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IStockAdjustmentRepository _repository;

    public CreateStockAdjustmentHandler(
        IStockAdjustmentRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
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


        // Remove Redis cache because product quantity changed
        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


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