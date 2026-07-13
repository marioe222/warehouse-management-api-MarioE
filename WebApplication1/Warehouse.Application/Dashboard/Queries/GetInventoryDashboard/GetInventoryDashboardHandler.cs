using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;

public class GetInventoryDashboardHandler
    : IRequestHandler<
        GetInventoryDashboardQuery,
        GetInventoryDashboardResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;


    public GetInventoryDashboardHandler(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }


    public async Task<GetInventoryDashboardResponse> Handle(
        GetInventoryDashboardQuery request,
        CancellationToken cancellationToken)
    {
        // Execute database calls sequentially
        // because EF Core DbContext is not thread-safe

        var products = await _productRepository.GetAll(
            cancellationToken
        );


        var suppliers = await _supplierRepository.GetAll(
            cancellationToken
        );


        return new GetInventoryDashboardResponse(
            TotalProducts: products.Count(),

            AvailableProducts: products.Count(p =>
                p.QuantityInStock > 0),

            LowStockProducts: products.Count(p =>
                p.QuantityInStock < 5),

            TotalSuppliers: suppliers.Count(),

            ActiveSuppliers: suppliers.Count(s =>
                s.IsActive)
        );
    }
}