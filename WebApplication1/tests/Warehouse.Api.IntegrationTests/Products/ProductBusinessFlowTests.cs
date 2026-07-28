using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;
using Warehouse.Api.IntegrationTests.Utilities.TestData;

namespace Warehouse.Api.IntegrationTests.Products;

public class ProductBusinessFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public ProductBusinessFlowTests(
        CustomWebApplicationFactory factory)
    {
        factory.ResetDatabase();

        _client = factory.CreateClient();
    }


    [Fact]
    public async Task Product_Full_Business_Flow_ShouldCompleteSuccessfully()
    {

        // Arrange test data using Builders

        var supplier = TestEntityFactory.CreateSupplier();

        var product = TestEntityFactory.CreateProduct();



        // Create Supplier

        var supplierResponse = await _client.PostAsJsonAsync(
            "/api/suppliers",
            new
            {
                name = supplier.Name,
                country = "Lebanon",
                contactEmail = supplier.ContactEmail,
                phoneNumber = "03111111"
            });


        supplierResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);



        var supplierId = await ExtractId(
            supplierResponse,
            "supplierId");




        // Create Product

        var productResponse = await _client.PostAsJsonAsync(
            "/api/products",
            new
            {
                name = product.Name,
                sku = product.Sku,
                description = product.Description,
                price = product.Price,
                quantityInStock = product.QuantityInStock,
                supplierName = product.SupplierName,
                expiryDate = product.ExpiryDate
            });


        productResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);



        var productId = await ExtractId(
            productResponse,
            "productId");




        // Assign Supplier

        var assignResponse = await _client.PostAsync(
            $"/api/products/{productId}/assign-supplier/{supplierId}",
            null);



        assignResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);




        // Upload Image

        using var content = new MultipartFormDataContent();


        var image = new ByteArrayContent(
        [
            255,
            216,
            255,
            224,
            1,
            2,
            3
        ]);


        image.Headers.ContentType =
            new MediaTypeHeaderValue("image/jpeg");


        content.Add(
            image,
            "file",
            "test-image.jpg");



        var uploadResponse = await _client.PostAsync(
            $"/api/products/{productId}/image",
            content);



        uploadResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);




        // Update Quantity

        var quantityResponse = await _client.PutAsJsonAsync(
            $"/api/products/{productId}/quantity",
            new
            {
                quantityInStock = 50
            });



        quantityResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);




        // Update Price

        var priceResponse = await _client.PutAsJsonAsync(
            $"/api/products/{productId}/price",
            new
            {
                price = 150
            });



        priceResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);




        // Archive Product

        var archiveResponse = await _client.DeleteAsync(
            $"/api/products/{productId}");



        archiveResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);




        // Verify Product

        var getResponse = await _client.GetAsync(
            $"/api/products/{productId}");



        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);



        var body = await getResponse.Content.ReadAsStringAsync();


        body.Should()
            .Contain("true");
    }





    private async Task<Guid> ExtractId(
        HttpResponseMessage response,
        string property)
    {
        var json = await response.Content.ReadAsStringAsync();


        using var document = JsonDocument.Parse(json);


        var root = document.RootElement;



        if (root.TryGetProperty("data", out var data))
        {

            if (data.TryGetProperty(property, out var customId))
            {
                return Guid.Parse(customId.GetString()!);
            }


            if (data.TryGetProperty("id", out var dataId))
            {
                return Guid.Parse(dataId.GetString()!);
            }

        }



        if (root.TryGetProperty(property, out var id))
        {
            return Guid.Parse(id.GetString()!);
        }



        if (root.TryGetProperty("id", out var rootId))
        {
            return Guid.Parse(rootId.GetString()!);
        }



        throw new Exception(
            $"Cannot extract id from response: {json}");
    }
}