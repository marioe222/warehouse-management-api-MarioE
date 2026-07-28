using System.Net.Http.Headers;

namespace Warehouse.Api.IntegrationTests.Utilities.Helpers;

public static class MultipartFormHelper
{
    public static MultipartFormDataContent Create(
        byte[] fileBytes,
        string fileName)
    {
        var form = new MultipartFormDataContent();


        var fileContent =
            new ByteArrayContent(fileBytes);


        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue("image/jpeg");


        form.Add(
            fileContent,
            "file",
            fileName);


        return form;
    }
}