using Warehouse.Domain.Entities;
using Warehouse.Api.IntegrationTests.Utilities.Builders;

namespace Warehouse.Api.IntegrationTests.Utilities.TestData;

public static class TestEntityFactory
{
    public static Supplier CreateSupplier()
    {
        return new SupplierBuilder()
            .WithName("Flow Supplier")
            .WithCountry("Lebanon")
            .WithEmail("flow@supplier.com")
            .WithPhoneNumber("03111111")
            .Build();
    }


    public static Product CreateProduct()
    {
        return new ProductBuilder()
            .WithName("Flow Product")
            .WithSku("FLOW-001")
            .WithPrice(100)
            .WithQuantity(20)
            .WithSupplierName("Flow Supplier")
            .WithExpiryDate(DateTime.UtcNow.AddMonths(5))
            .Build();
    }
}