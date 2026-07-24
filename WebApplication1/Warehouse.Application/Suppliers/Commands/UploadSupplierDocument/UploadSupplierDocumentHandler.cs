using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Suppliers.Commands.UploadSupplierDocument;


public class UploadSupplierDocumentHandler
    : IRequestHandler<
        UploadSupplierDocumentCommand,
        UploadSupplierDocumentResponse>
{

    private readonly ISupplierRepository _supplierRepository;
    private readonly IStorageService _storageService;
    private readonly IFileMetadataRepository _fileRepository;
    private readonly IDistributedCache _cache;



    public UploadSupplierDocumentHandler(
        ISupplierRepository supplierRepository,
        IStorageService storageService,
        IFileMetadataRepository fileRepository,
        IDistributedCache cache)
    {
        _supplierRepository = supplierRepository;
        _storageService = storageService;
        _fileRepository = fileRepository;
        _cache = cache;
    }



    public async Task<UploadSupplierDocumentResponse> Handle(
        UploadSupplierDocumentCommand request,
        CancellationToken cancellationToken)
    {


        // Check supplier exists

        var supplier =
            await _supplierRepository.GetById(
                request.SupplierId,
                cancellationToken);



        if (supplier == null)
        {
            return new UploadSupplierDocumentResponse(false);
        }



        // File size validation
        // 10 MB maximum

        const long maxSize =
            10 * 1024 * 1024;


        if(request.File.Length > maxSize)
        {
            throw new Exception(
                "File size cannot exceed 10MB");
        }



        // Allowed document types

        var allowedTypes = new[]
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };



        if(!allowedTypes.Contains(
            request.File.ContentType))
        {
            throw new Exception(
                "Only PDF and Word documents are allowed");
        }




        // Upload to MinIO

        await using var stream =
            request.File.OpenReadStream();



        var objectKey =
            await _storageService.UploadAsync(
                stream,
                request.File.FileName,
                request.File.ContentType,
                cancellationToken);




        // Save metadata

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

            SupplierId =
                request.SupplierId,

            UploadedDate =
                DateTime.UtcNow
        };



        await _fileRepository.AddAsync(
            metadata,
            cancellationToken);



        // Clear supplier cache

        await _cache.RemoveAsync(
            $"supplier:{request.SupplierId}",
            cancellationToken);


        await _cache.RemoveAsync(
            "suppliers",
            cancellationToken);



        return new UploadSupplierDocumentResponse(true);
    }
}