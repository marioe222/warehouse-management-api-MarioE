using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Application.Products.Commands.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();