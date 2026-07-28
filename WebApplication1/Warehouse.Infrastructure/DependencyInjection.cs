using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Firebase;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Infrastructure.Storage;


namespace Warehouse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        // Firebase
        var firebaseEnabled = configuration
            .GetValue<bool>("Firebase:Enabled");


        if (firebaseEnabled)
        {
            FirebaseInitializer.Initialize();
        }


        services.AddScoped<FirebaseAdminService>();


        // Database
        services.AddDbContext<WarehouseDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });


        // Repositories

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();

        services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();


        // MinIO Client

        services.AddMinio(client =>
        {
            client
                .WithEndpoint(
                    configuration["Minio:Endpoint"]!)
                .WithCredentials(
                    configuration["Minio:AccessKey"]!,
                    configuration["Minio:SecretKey"]!)
                .WithSSL(
                    bool.Parse(
                        configuration["Minio:UseSSL"] ?? "false"))
                .Build();
        });


        // Storage Service

        services.AddScoped<IStorageService, MinioStorageService>();

        services.AddScoped<
            IFileMetadataRepository,
            FileMetadataRepository>();


        return services;
    }
}