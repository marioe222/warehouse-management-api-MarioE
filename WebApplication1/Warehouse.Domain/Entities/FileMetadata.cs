namespace Warehouse.Domain.Entities;

public class FileMetadata
{
    public Guid Id { get; set; }


    public string FileName { get; set; } = string.Empty;


    public string ObjectKey { get; set; } = string.Empty;


    public string ContentType { get; set; } = string.Empty;


    public long Size { get; set; }


    // Product image relation
    public Guid? ProductId { get; set; }


    // Supplier document relation
    public Guid? SupplierId { get; set; }


    public DateTime UploadedDate { get; set; }
}