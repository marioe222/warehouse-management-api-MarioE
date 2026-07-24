using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(
        DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }


    public DbSet<Product> Products { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<StockAdjustment> StockAdjustments { get; set; }
    
    public DbSet<FileMetadata> FileMetadata { get; set; }
}