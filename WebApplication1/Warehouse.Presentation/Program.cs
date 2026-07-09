using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Application.Products.Commands.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


//Database Connection
builder.Services.AddDbContext<WarehouseDbFirstContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


//MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );
});


// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();


// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();