using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.IntegrationEvents.Events;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public class UploadProductImageHandler
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IFileMetadataRepository _fileRepository;
    private readonly IEventPublisher _publisher;


    public UploadProductImageHandler(
        IProductRepository repository,
        IDistributedCache cache,
        IStorageService storageService,
        IFileMetadataRepository fileRepository,
        IEventPublisher publisher)
    {
        _repository = repository;
        _cache = cache;
        _storageService = storageService;
        _fileRepository = fileRepository;
        _publisher = publisher;
    }


    public async Task<UploadProductImageResponse> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {

        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
        {
            return new UploadProductImageResponse(false);
        }




        const long maxFileSize = 5 * 1024 * 1024;


        if (request.File.Length > maxFileSize)
        {
            throw new Exception(
                "File size cannot exceed 5MB");
        }




        var allowedTypes = new[]
        {
            "image/jpeg",
            "image/png"
        };


        if (!allowedTypes.Contains(request.File.ContentType))
        {
            throw new Exception(
                "Only JPG and PNG images are allowed");
        }




        await using var stream =
            request.File.OpenReadStream();


        var objectKey =
            await _storageService.UploadAsync(
                stream,
                request.File.FileName,
                request.File.ContentType,
                cancellationToken
            );




        var metadata = new FileMetadata
        {
            Id = Guid.NewGuid(),

            FileName = request.File.FileName,

            ObjectKey = objectKey,

            ContentType = request.File.ContentType,

            Size = request.File.Length,

            ProductId = request.ProductId,

            UploadedDate = DateTime.UtcNow
        };


        await _fileRepository.AddAsync(
            metadata,
            cancellationToken
        );



        await _publisher.PublishAsync(
            new WarehouseFileUploaded
            {
                EventId = Guid.NewGuid(),

                FileName = request.File.FileName,

                FileUrl = objectKey,

                UploadedAt = DateTime.UtcNow
            },
            "file.uploaded"
        );




        await _cache.RemoveAsync(
            $"product:{request.ProductId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:True",
            cancellationToken);


        await _cache.RemoveAsync(
            "products:False",
            cancellationToken);



        return new UploadProductImageResponse(true);
    }
}