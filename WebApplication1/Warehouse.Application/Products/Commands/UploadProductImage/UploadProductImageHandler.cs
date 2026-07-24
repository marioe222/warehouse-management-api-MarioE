using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public class UploadProductImageHandler
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResponse>
{
    private readonly IDistributedCache _cache;
    private readonly IProductRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IFileMetadataRepository _fileRepository;


    public UploadProductImageHandler(
        IProductRepository repository,
        IDistributedCache cache,
        IStorageService storageService,
        IFileMetadataRepository fileRepository)
    {
        _repository = repository;
        _cache = cache;
        _storageService = storageService;
        _fileRepository = fileRepository;
    }


    public async Task<UploadProductImageResponse> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        // Check product exists

        var product = await _repository.GetById(
            request.ProductId,
            cancellationToken
        );


        if (product == null)
        {
            return new UploadProductImageResponse(false);
        }



        // File size validation (5 MB)

        const long maxFileSize =
            5 * 1024 * 1024;


        if (request.File.Length > maxFileSize)
        {
            throw new Exception(
                "File size cannot exceed 5MB");
        }



        // Content type validation

        var allowedTypes = new[]
        {
            "image/jpeg",
            "image/png"
        };


        if (!allowedTypes.Contains(
                request.File.ContentType))
        {
            throw new Exception(
                "Only JPG and PNG images are allowed");
        }



        // Upload file to MinIO

        await using var stream =
            request.File.OpenReadStream();


        var objectKey =
            await _storageService.UploadAsync(
                stream,
                request.File.FileName,
                request.File.ContentType,
                cancellationToken
            );



        // Save metadata in PostgreSQL

        var metadata = new FileMetadata
        {
            Id = Guid.NewGuid(),

            FileName =
                request.File.FileName,

            ObjectKey =
                objectKey,

            ContentType =
                request.File.ContentType,

            Size =
                request.File.Length,

            ProductId =
                request.ProductId,

            UploadedDate =
                DateTime.UtcNow
        };


        await _fileRepository.AddAsync(
            metadata,
            cancellationToken
        );



        // Clear Redis cache

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