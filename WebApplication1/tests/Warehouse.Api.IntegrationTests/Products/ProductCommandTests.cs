using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Products;

public class ProductCommandTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProductCommandTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;

        // Reset the database before each test
        _factory.ResetDatabase();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreated()
    {
        var request = new
        {
            name = "Created Integration Product",
            sku = "CREATE-001",
            description = "Created from integration test",
            price = 100,
            quantityInStock = 20,
            supplierName = "Test Supplier",
            expiryDate = DateTime.UtcNow.AddMonths(6)
        };

        var response = await _client.PostAsJsonAsync(
            "/api/products",
            request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateQuantity_ShouldReturnOk()
    {
        var productId = _factory.SeededProductId;

        var request = new
        {
            quantityInStock = 75
        };

        var response = await _client.PostAsJsonAsync(
            $"/api/products/{productId}/quantity",
            request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdatePrice_ShouldReturnOk()
    {
        var productId = _factory.SeededProductId;

        var request = new
        {
            price = 199.99m
        };

        var response = await _client.PostAsJsonAsync(
            $"/api/products/{productId}/price",
            request);

        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine("========== RESPONSE ==========");
        Console.WriteLine(body);
        Console.WriteLine("==============================");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProduct_ShouldArchiveProduct()
    {
        var productId = _factory.SeededProductId;

        var response = await _client.DeleteAsync(
            $"/api/products/{productId}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<Warehouse.Infrastructure.Data.WarehouseDbContext>();

        var product = await db.Products.FindAsync(productId);

        product.Should().NotBeNull();

        product!.IsArchived.Should().BeTrue();
    }
}