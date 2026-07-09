using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Warehouse.Domain.Interface;

using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Repositories;

using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Infrastructure.Models;


var builder = WebApplication.CreateBuilder(args);


// Controllers 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });



// PostgreSQL Database Connection (DB First)
builder.Services.AddDbContext<WarehouseDbFirstContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});



// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );
});



// Dependency Injection - Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();



// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();



var app = builder.Build();



// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();