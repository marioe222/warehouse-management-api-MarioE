using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Suppliers;


public class SupplierEndpointTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    private readonly CustomWebApplicationFactory _factory;



    public SupplierEndpointTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;

        // reset database before each test class
        _factory.ResetDatabase();

        _client = factory.CreateClient();
    }



    [Fact]
    public async Task CreateSupplier_ShouldReturnCreated()
    {
        var request = new
        {
            name = "New Integration Supplier",
            country = "Lebanon",
            contactEmail = "new@supplier.com",
            phoneNumber = "03123456"
        };


        var response = await _client.PostAsJsonAsync(
            "/api/suppliers",
            request);



        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }





    [Fact]
    public async Task GetSupplier_ShouldReturnSupplier()
    {
        var supplierId = _factory.SeededSupplierId;


        var response = await _client.GetAsync(
            $"/api/suppliers/{supplierId}");



        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine(body);



        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }





    [Fact]
    public async Task DeactivateSupplier_ShouldReturnOk()
    {
        var supplierId = _factory.SeededSupplierId;



        var response = await _client.DeleteAsync(
            $"/api/suppliers/{supplierId}");



        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }





    [Fact]
    public async Task AssignSupplierToProduct_ShouldReturnOk()
    {
        var productId = _factory.SeededProductId;

        var supplierId = _factory.SeededSupplierId;



        var response = await _client.PostAsync(
            $"/api/products/{productId}/assign-supplier/{supplierId}",
            null);



        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine(body);



        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }
}