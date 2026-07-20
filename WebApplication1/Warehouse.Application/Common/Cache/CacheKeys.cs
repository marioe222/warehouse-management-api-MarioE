namespace Warehouse.Application.Common.Cache;

public static class CacheKeys
{
    public static string Product(Guid id)
        => $"product:{id}";

    public static string Products(bool onlyAvailable)
        => $"products:{onlyAvailable}";
}