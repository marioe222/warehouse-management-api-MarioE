using Warehouse.Domain.Entities;

namespace Warehouse.Api.IntegrationTests.Utilities.Builders;

public class ProductBuilder
{
    private string _name = "Test Product";
    private string _sku = "TEST-001";
    private string _description = "Test Description";
    private decimal _price = 100;
    private int _quantity = 10;
    private string? _supplierName = null;
    private DateTime? _expiryDate = DateTime.UtcNow.AddMonths(6);


    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }


    public ProductBuilder WithSku(string sku)
    {
        _sku = sku;
        return this;
    }


    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }


    public ProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }


    public ProductBuilder WithSupplierName(string supplierName)
    {
        _supplierName = supplierName;
        return this;
    }


    public ProductBuilder WithExpiryDate(DateTime expiryDate)
    {
        _expiryDate = expiryDate;
        return this;
    }


    public Product Build()
    {
        return new Product(
            _name,
            _sku,
            _description,
            _price,
            _quantity,
            _supplierName,
            _expiryDate
        );
    }
}