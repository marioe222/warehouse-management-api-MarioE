namespace Warehouse.IntegrationEvents.Events;

public class WarehouseFileUploaded
{
    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}