using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class FileMetadataRepository : IFileMetadataRepository
{
    private readonly WarehouseDbContext _context;


    public FileMetadataRepository(
        WarehouseDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken = default)
    {
        await _context.FileMetadata.AddAsync(
            fileMetadata,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<FileMetadata?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.FileMetadata
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }


    public async Task<FileMetadata?> GetByObjectKeyAsync(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        return await _context.FileMetadata
            .FirstOrDefaultAsync(
                x => x.ObjectKey == objectKey,
                cancellationToken);
    }
}