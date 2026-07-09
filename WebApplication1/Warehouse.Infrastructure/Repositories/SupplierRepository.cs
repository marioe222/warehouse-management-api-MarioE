using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Interface;

using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Models;
using DbSupplier = Warehouse.Infrastructure.Models.Supplier;
using Supplier = Warehouse.Domain.Entities.Supplier;


namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbFirstContext _context;


    public SupplierRepository(WarehouseDbFirstContext context)
    {
        _context = context;
    }



    public async Task Add(Supplier supplier)
    {
        var dbSupplier = new DbSupplier
        {
            Name = supplier.Name,
            Contactemail = supplier.ContactEmail,
            Isactive = supplier.IsActive
        };


        await _context.Suppliers.AddAsync(dbSupplier);

        await _context.SaveChangesAsync();
    }



    public async Task<Supplier?> GetById(int id)
    {
        var dbSupplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Supplierid == id);


        if (dbSupplier == null)
            return null;


        return new Supplier(
            dbSupplier.Name,
            dbSupplier.Contactemail ?? ""
        );
    }



    public async Task<IEnumerable<Supplier>> GetAll()
    {
        var suppliers = await _context.Suppliers
            .ToListAsync();


        return suppliers.Select(s =>
            new Supplier(
                s.Name,
                s.Contactemail ?? ""
            )
        );
    }



    public async Task Update(Supplier supplier)
    {
        var dbSupplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Supplierid == supplier.Id);


        if (dbSupplier == null)
            return;


        dbSupplier.Name = supplier.Name;
        dbSupplier.Contactemail = supplier.ContactEmail;
        dbSupplier.Isactive = supplier.IsActive;


        await _context.SaveChangesAsync();
    }
}