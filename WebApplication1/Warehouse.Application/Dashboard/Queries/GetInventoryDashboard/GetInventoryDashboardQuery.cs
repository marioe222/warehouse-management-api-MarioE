using MediatR;

namespace Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;

public record GetInventoryDashboardQuery
    : IRequest<GetInventoryDashboardResponse>;