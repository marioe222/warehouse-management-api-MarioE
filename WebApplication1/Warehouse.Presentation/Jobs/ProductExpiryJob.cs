using Warehouse.Domain.Interface;

namespace Warehouse.Presentation.Jobs;

public class ProductExpiryJob
{
    private readonly ILogger<ProductExpiryJob> _logger;
    private readonly IProductRepository _repository;


    public ProductExpiryJob(
        ILogger<ProductExpiryJob> logger,
        IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    public async Task CheckProductsAsync(
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(
            DateTime.Today);


        var products = await _repository.GetExpiringProducts(
            today,
            cancellationToken);


        var expiredProducts = products
            .Where(p =>
                p.ExpiryDate.HasValue &&
                DateOnly.FromDateTime(p.ExpiryDate.Value) < today)
            .ToList();


        var soonToExpireProducts = products
            .Where(p =>
                p.ExpiryDate.HasValue &&
                DateOnly.FromDateTime(p.ExpiryDate.Value) >= today &&
                DateOnly.FromDateTime(p.ExpiryDate.Value) <= today.AddDays(30))
            .ToList();


        _logger.LogInformation(
            "Expired products count: {Count}",
            expiredProducts.Count);


        foreach (var product in expiredProducts)
        {
            _logger.LogWarning(
                "Expired product: {Name}",
                product.Name);
        }


        _logger.LogInformation(
            "Soon-to-expire products count: {Count}",
            soonToExpireProducts.Count);


        foreach (var product in soonToExpireProducts)
        {
            _logger.LogInformation(
                "Soon to expire product: {Name}",
                product.Name);
        }
    }
}