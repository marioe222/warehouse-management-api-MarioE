using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Warehouse.Api.IntegrationTests.Authentication;
using Warehouse.Api.IntegrationTests.Fakes;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Api.IntegrationTests.Factories;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    public Guid SeededProductId { get; private set; }

    public Guid SeededSupplierId { get; private set; }


    private readonly string _databaseName =
        $"WarehouseIntegrationTestDb_{Guid.NewGuid()}";



    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");


        builder.ConfigureServices(services =>
        {

            var dbDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<WarehouseDbContext>));


            if (dbDescriptor != null)
            {
                services.Remove(dbDescriptor);
            }



            services.AddDbContext<WarehouseDbContext>(options =>
            {
                options.UseInMemoryDatabase(
                    _databaseName);
            });



            var cacheDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IDistributedCache));


            if (cacheDescriptor != null)
            {
                services.Remove(cacheDescriptor);
            }



            services.AddSingleton<IDistributedCache>(
                new MemoryDistributedCache(
                    new OptionsWrapper<MemoryDistributedCacheOptions>(
                        new MemoryDistributedCacheOptions())));



            var storageDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IStorageService));


            if (storageDescriptor != null)
            {
                services.Remove(storageDescriptor);
            }



            services.AddScoped<IStorageService,
                FakeStorageService>();



            var publisherDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IEventPublisher));


            if (publisherDescriptor != null)
            {
                services.Remove(publisherDescriptor);
            }



            services.AddScoped<IEventPublisher,
                FakeEventPublisher>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";

                options.DefaultChallengeScheme = "Test";

            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "Test",
                options => { });



            using var scope = services
                .BuildServiceProvider()
                .CreateScope();



            var dbContext = scope.ServiceProvider
                .GetRequiredService<WarehouseDbContext>();



            dbContext.Database.EnsureCreated();



            SeedData(dbContext);

        });
    }





    private void SeedData(
        WarehouseDbContext dbContext)
    {

        dbContext.Products.RemoveRange(
            dbContext.Products.ToList());


        dbContext.Suppliers.RemoveRange(
            dbContext.Suppliers.ToList());


        dbContext.SaveChanges();




        var supplier = new Supplier(
            name: "Integration Test Supplier",
            contactEmail: "supplier@test.com"
        );


        dbContext.Suppliers.Add(supplier);




        var product = new Product(
            name: "Integration Test Product",
            sku: "TEST-001",
            description: "Product created for integration testing",
            price: 25.99m,
            quantityInStock: 50,
            supplierName: "Integration Test Supplier",
            expiryDate: DateTime.UtcNow.AddMonths(6)
        );




        var lowStockProduct = new Product(
            name: "Low Stock Product",
            sku: "LOW-001",
            description: "Product with low quantity",
            price: 15.99m,
            quantityInStock: 1,
            supplierName: "Integration Test Supplier",
            expiryDate: DateTime.UtcNow.AddMonths(3)
        );




        product.AssignSupplier(supplier);



        dbContext.Products.Add(product);

        dbContext.Products.Add(lowStockProduct);



        dbContext.SaveChanges();




        SeededSupplierId = supplier.Id;

        SeededProductId = product.Id;

    }





    public void ResetDatabase()
    {
        using var scope = Services.CreateScope();


        var db = scope.ServiceProvider
            .GetRequiredService<WarehouseDbContext>();


        db.Products.RemoveRange(
            db.Products.ToList());


        db.Suppliers.RemoveRange(
            db.Suppliers.ToList());


        db.SaveChanges();



        SeedData(db);
    }
}