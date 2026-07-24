using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Warehouse.Application.Interfaces;

namespace Warehouse.Infrastructure.Storage;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;


    public MinioStorageService(
        IMinioClient minioClient,
        IConfiguration configuration)
    {
        _minioClient = minioClient;

        _bucketName =
            configuration["Minio:BucketName"]!;
    }


    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var objectName =
            $"{Guid.NewGuid()}_{fileName}";


        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType),
            cancellationToken
        );


        return objectName;
    }


    public async Task<Stream> DownloadAsync(
        string objectName,
        CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();


        await _minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                }),
            cancellationToken
        );


        memoryStream.Position = 0;


        return memoryStream;
    }


    public async Task DeleteAsync(
        string objectName,
        CancellationToken cancellationToken = default)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName),
            cancellationToken
        );
    }
}