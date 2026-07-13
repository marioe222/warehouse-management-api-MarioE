namespace Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;

public record GetInventoryDashboardResponse(
    int TotalProducts,
    int AvailableProducts,
    int LowStockProducts,
    int TotalSuppliers,
    int ActiveSuppliers
);