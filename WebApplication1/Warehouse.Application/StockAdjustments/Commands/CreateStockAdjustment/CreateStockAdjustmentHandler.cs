using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.IntegrationEvents.Events;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentHandler
    : IRequestHandler<CreateStockAdjustmentCommand, CreateStockAdjustmentResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IStockAdjustmentRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;


    public CreateStockAdjustmentHandler(
        IStockAdjustmentRepository repository,
        IProductRepository productRepository,
        IDistributedCache cache,
        IEventPublisher eventPublisher)
    {
        _repository = repository;
        _productRepository = productRepository;
        _cache = cache;
        _eventPublisher = eventPublisher;
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
            cancellationToken);


        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);


        var product = await _productRepository.GetById(
            request.ProductId,
            cancellationToken);


        const int minimumQuantity = 10;

        if (product != null &&
            product.QuantityInStock <= minimumQuantity)
        {
            await _eventPublisher.PublishAsync(
                new StockLowDetected
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    CurrentQuantity = product.QuantityInStock,
                    MinimumQuantity = minimumQuantity,
                    DetectedAt = DateTime.UtcNow
                },
                "stock.low");
        }


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