namespace Warehouse.Application.Common;

public static class CacheKeys
{
    public static string Product(Guid id)
        => $"product:{id}";
}