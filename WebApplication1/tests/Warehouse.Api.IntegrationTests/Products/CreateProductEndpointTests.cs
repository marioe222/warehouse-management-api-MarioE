using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Products;

public class CreateProductEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public CreateProductEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CreateProduct_ShouldReturnCreated()
    {
        var request = new
        {
            name = "New Integration Product",
            sku = "NEW-001",
            description = "Created from integration test",
            price = 99.99m,
            quantityInStock = 20,
            supplierName = "Test Supplier",
            expiryDate = DateTime.UtcNow.AddMonths(12)
        };


        var response = await _client
            .PostAsJsonAsync(
                "/api/products",
                request);


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }
}