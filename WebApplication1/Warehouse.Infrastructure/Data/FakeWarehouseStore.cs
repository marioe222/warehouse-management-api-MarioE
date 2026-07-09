using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Data;

public static class FakeWarehouseStore
{
    public static List<Supplier> Suppliers { get; } = new();

    public static List<Product> Products { get; } = new()
    {
        new Product(
            "Laptop",
            "LAP-001",
            "High performance laptop",
            1200m,
            15,
            "TechSupplier",
            null
        ),

        new Product(
            "Mouse",
            "MOU-002",
            "Wireless mouse",
            25m,
            100,
            "AccessoryWorld",
            null
        ),

        new Product(
            "Keyboard",
            "KEY-003",
            "Mechanical keyboard",
            80m,
            60,
            "AccessoryWorld",
            null
        ),

        new Product(
            "Scanner",
            "SCA-004",
            "Office document scanner",
            300m,
            10,
            "OfficePro",
            null
        ),

        new Product(
            "Printer",
            "PRI-005",
            "Laser printer",
            450m,
            8,
            "OfficePro",
            null
        ),

        new Product(
            "Monitor",
            "MON-006",
            "24-inch LED monitor",
            220m,
            30,
            "DisplayTech",
            null
        ),

        new Product(
            "Webcam",
            "WEB-007",
            "HD webcam",
            70m,
            40,
            "AccessoryWorld",
            null
        ),

        new Product(
            "Headset",
            "HEA-008",
            "Noise cancelling headset",
            95m,
            35,
            "SoundGear",
            null
        ),

        new Product(
            "USB Flash Drive",
            "USB-009",
            "64GB USB 3.0 flash drive",
            18m,
            200,
            "StoragePlus",
            null
        ),

        new Product(
            "External Hard Drive",
            "HDD-010",
            "1TB external hard drive",
            110m,
            25,
            "StoragePlus",
            null
        )
    };
}