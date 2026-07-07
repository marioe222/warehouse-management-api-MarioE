using System;
using System.Collections.Generic;
using Warehouse.Presentation.modules;

namespace Warehouse.Presentation.Data
{
    public static class FakeWarehouseStore
    {
        public static List<Supplier> Suppliers = new();
        public static List<Products> Products { get; } = new()
        {
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Laptop",
                Sku = "LAP-001",
                Description = "High performance laptop",
                Price = 1200m,
                QuantityInStock = 15,
                SupplierName = "TechSupplier",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Mouse",
                Sku = "MOU-002",
                Description = "Wireless mouse",
                Price = 25m,
                QuantityInStock = 100,
                SupplierName = "AccessoryWorld",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Keyboard",
                Sku = "KEY-003",
                Description = "Mechanical keyboard",
                Price = 80m,
                QuantityInStock = 60,
                SupplierName = "AccessoryWorld",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Scanner",
                Sku = "SCA-004",
                Description = "Office document scanner",
                Price = 300m,
                QuantityInStock = 10,
                SupplierName = "OfficePro",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Printer",
                Sku = "PRI-005",
                Description = "Laser printer",
                Price = 450m,
                QuantityInStock = 8,
                SupplierName = "OfficePro",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Monitor",
                Sku = "MON-006",
                Description = "24-inch LED monitor",
                Price = 220m,
                QuantityInStock = 30,
                SupplierName = "DisplayTech",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Webcam",
                Sku = "WEB-007",
                Description = "HD webcam",
                Price = 70m,
                QuantityInStock = 40,
                SupplierName = "AccessoryWorld",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "Headset",
                Sku = "HEA-008",
                Description = "Noise cancelling headset",
                Price = 95m,
                QuantityInStock = 35,
                SupplierName = "SoundGear",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "USB Flash Drive",
                Sku = "USB-009",
                Description = "64GB USB 3.0 flash drive",
                Price = 18m,
                QuantityInStock = 200,
                SupplierName = "StoragePlus",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
            new Products
            {
                Id = Guid.NewGuid(),
                Name = "External Hard Drive",
                Sku = "HDD-010",
                Description = "1TB external hard drive",
                Price = 110m,
                QuantityInStock = 25,
                SupplierName = "StoragePlus",
                ExpiryDate = null,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            }
        };
    }
}