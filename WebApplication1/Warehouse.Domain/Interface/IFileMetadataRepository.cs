using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IFileMetadataRepository
{
    Task AddAsync(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);

    Task<FileMetadata?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<FileMetadata?> GetByObjectKeyAsync(
        string objectKey,
        CancellationToken cancellationToken = default);
}