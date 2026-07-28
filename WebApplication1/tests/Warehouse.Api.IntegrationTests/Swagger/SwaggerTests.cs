using System.Net;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Swagger;


public class SwaggerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public SwaggerTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }



    [Fact]
    public async Task Swagger_Json_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync(
            "/swagger/v1/swagger.json");


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);


        response.Content.Headers.ContentType!
            .MediaType
            .Should()
            .Be("application/json");
    }
}