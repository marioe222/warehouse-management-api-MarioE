namespace Warehouse.Application.Common.Cache;

public static class CacheKeys
{
    public static string Product(Guid productId)
        => $"product:{productId}";

    public static string Products(bool onlyAvailable)
        => $"products:{onlyAvailable}";

    public static string SearchProducts(
        string? name,
        string? supplier)
        => $"products:{name}:{supplier}";

    public static string Supplier(Guid supplierId)
        => $"supplier:{supplierId}";

    public const string Suppliers = "suppliers";
}