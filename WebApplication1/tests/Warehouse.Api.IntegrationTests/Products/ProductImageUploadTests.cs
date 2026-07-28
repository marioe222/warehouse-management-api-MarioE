using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.Factories;

namespace Warehouse.Api.IntegrationTests.Products;

public class ProductImageUploadTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;


    public ProductImageUploadTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _factory.ResetDatabase();

        _client = factory.CreateClient();

        // Fake admin authentication for [Authorize]
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Test");
    }



    [Fact]
    public async Task Upload_Jpg_Image_ShouldReturnOk()
    {
        var response = await UploadFile(
            "test.jpg",
            "image/jpeg",
            new byte[]
            {
                255,216,255,224,1,2,3
            });


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }





    [Fact]
    public async Task Upload_Png_Image_ShouldReturnOk()
    {
        var response = await UploadFile(
            "test.png",
            "image/png",
            new byte[]
            {
                137,80,78,71,1,2,3
            });


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }





    [Fact]
    public async Task Upload_Txt_File_ShouldReject()
    {
        var response = await UploadFile(
            "test.txt",
            "text/plain",
            System.Text.Encoding.UTF8
                .GetBytes("hello"));


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.InternalServerError);
    }





    [Fact]
    public async Task Upload_Oversized_File_ShouldReject()
    {
        var largeFile =
            new byte[6 * 1024 * 1024];


        var response = await UploadFile(
            "large.jpg",
            "image/jpeg",
            largeFile);


        response.StatusCode
            .Should()
            .Be(HttpStatusCode.InternalServerError);
    }





    private async Task<HttpResponseMessage> UploadFile(
        string fileName,
        string contentType,
        byte[] fileBytes)
    {
        var productId =
            _factory.SeededProductId;



        using var content =
            new MultipartFormDataContent();



        var fileContent =
            new ByteArrayContent(fileBytes);



        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(
                contentType);



        content.Add(
            fileContent,
            "file",
            fileName);



        return await _client.PostAsync(
            $"/api/products/{productId}/image",
            content);
    }
}