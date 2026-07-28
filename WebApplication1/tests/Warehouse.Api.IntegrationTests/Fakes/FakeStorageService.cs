using Warehouse.Application.Interfaces;

namespace Warehouse.Api.IntegrationTests.Fakes;

public class FakeStorageService : IStorageService
{
    public Task<string> UploadAsync(
        Stream file,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            $"test-storage/{fileName}");
    }



    public Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }



    public Task<Stream> DownloadAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        Stream stream = new MemoryStream();

        return Task.FromResult(stream);
    }
}