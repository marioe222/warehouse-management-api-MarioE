using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Interface;
using Warehouse.IntegrationEvents.Events;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentHandler
    : IRequestHandler<CreateStockAdjustmentCommand, CreateStockAdjustmentResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IStockAdjustmentRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CreateStockAdjustmentHandler> _logger;


    public CreateStockAdjustmentHandler(
        IStockAdjustmentRepository repository,
        IProductRepository productRepository,
        IDistributedCache cache,
        IEventPublisher eventPublisher,
        ILogger<CreateStockAdjustmentHandler> logger)
    {
        _repository = repository;
        _productRepository = productRepository;
        _cache = cache;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }


    public async Task<CreateStockAdjustmentResponse> Handle(
        CreateStockAdjustmentCommand request,
        CancellationToken cancellationToken)
    {
        var adjustment = new Warehouse.Domain.Entities.StockAdjustment
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


        var product = await _productRepository.GetById(
            request.ProductId,
            cancellationToken);


        if (product != null)
        {
            _logger.LogInformation(
                "Product quantity before update: {Quantity}",
                product.QuantityInStock);


            var newQuantity =
                product.QuantityInStock + request.QuantityChange;


            product.UpdateQuantity(newQuantity);


            _logger.LogInformation(
                "Product quantity after update: {Quantity}",
                product.QuantityInStock);


            await _productRepository.Update(
                product,
                cancellationToken);


            const int minimumQuantity = 10;


            if (product.QuantityInStock <= minimumQuantity)
            {
                _logger.LogInformation(
                    "Low stock detected for product {ProductName}. Publishing event.",
                    product.Name);


                await _eventPublisher.PublishAsync(
                    new StockLowDetected
                    {
                        EventId = Guid.NewGuid(),
                        ProductId = product.Id,
                        ProductName = product.Name,
                        CurrentQuantity = product.QuantityInStock,
                        MinimumQuantity = minimumQuantity,
                        DetectedAt = DateTime.UtcNow
                    },
                    "stock.low");


                _logger.LogInformation(
                    "StockLowDetected event published successfully.");
            }
        }


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