using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Interface;

using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Models;
using DbProduct = Warehouse.Infrastructure.Models.Product;
using Product = Warehouse.Domain.Entities.Product;


namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbFirstContext _context;


    public ProductRepository(WarehouseDbFirstContext context)
    {
        _context = context;
    }



    public async Task Add(Product product)
    {
        var dbProduct = new DbProduct
        {
            Name = product.Name,
            Price = product.Price,
            Quantity = product.QuantityInStock,
            Supplierid = product.SupplierId,
            Expirydate = product.ExpiryDate.HasValue
                ? DateOnly.FromDateTime(product.ExpiryDate.Value)
                : null
        };


        await _context.Products.AddAsync(dbProduct);

        await _context.SaveChangesAsync();
    }



    public async Task<Product?> GetById(int id)
    {
        var dbProduct = await _context.Products
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Productid == id);


        if (dbProduct == null)
            return null;


        return new Product(
            dbProduct.Name,
            "",
            "",
            dbProduct.Price ?? 0,
            dbProduct.Quantity ?? 0,
            dbProduct.Supplierid ?? 0,
            dbProduct.Supplier?.Name ?? "",
            dbProduct.Expirydate.HasValue
                ? dbProduct.Expirydate.Value.ToDateTime(TimeOnly.MinValue)
                : null
        );
    }



    public async Task<IEnumerable<Product>> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Supplier)
            .ToListAsync();


        return products.Select(p =>
            new Product(
                p.Name,
                "",
                "",
                p.Price ?? 0,
                p.Quantity ?? 0,
                p.Supplierid ?? 0,
                p.Supplier?.Name ?? "",
                p.Expirydate.HasValue
                    ? p.Expirydate.Value.ToDateTime(TimeOnly.MinValue)
                    : null
            )
        );
    }



    public async Task Update(Product product)
    {
        var dbProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Productid == product.Id);


        if (dbProduct == null)
            return;


        dbProduct.Name = product.Name;
        dbProduct.Price = product.Price;
        dbProduct.Quantity = product.QuantityInStock;
        dbProduct.Supplierid = product.SupplierId;

        dbProduct.Expirydate = product.ExpiryDate.HasValue
            ? DateOnly.FromDateTime(product.ExpiryDate.Value)
            : null;


        await _context.SaveChangesAsync();
    }
}