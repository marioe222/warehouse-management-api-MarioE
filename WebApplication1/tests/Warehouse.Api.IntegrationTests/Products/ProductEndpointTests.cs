using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Products;

public class ProductEndpointTests 
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;


    public ProductEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _factory.ResetDatabase();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ShouldReturnSeededProducts()
    {
        var response = await _client.GetAsync("/api/products");

        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine("========== RESPONSE ==========");
        Console.WriteLine($"Status Code: {(int)response.StatusCode} ({response.StatusCode})");
        Console.WriteLine(body);
        Console.WriteLine("==============================");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task GetProductById_ShouldReturnSeededProduct()
    {
        var productId = _factory.SeededProductId;


        var response = await _client
            .GetAsync($"/api/products/{productId}");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var product = await response.Content
            .ReadFromJsonAsync<ProductResponse>();


        product.Should()
            .NotBeNull();


        product!.Name
            .Should()
            .Be("Integration Test Product");
    }


    [Fact]
    public async Task GetProductById_WithInvalidId_ShouldReturnNotFound()
    {
        var invalidId = Guid.NewGuid();


        var response = await _client
            .GetAsync($"/api/products/{invalidId}");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task SearchProducts_ByName_ShouldReturnMatches()
    {
        var response = await _client
            .GetAsync("/api/products/search?name=Integration");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var products = await response.Content
            .ReadFromJsonAsync<List<object>>();


        products.Should()
            .NotBeNull();


        products.Should()
            .NotBeEmpty();
    }


    [Fact]
    public async Task GetProducts_OnlyAvailable_ShouldReturnProducts()
    {
        var response = await _client
            .GetAsync("/api/products?onlyAvailable=true");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        var products = await response.Content
            .ReadFromJsonAsync<List<object>>();


        products.Should()
            .NotBeNull();


        products.Should()
            .NotBeEmpty();
    }
}