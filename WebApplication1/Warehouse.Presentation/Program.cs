using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Application.Products.Commands;
var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );
});
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<ISupplierRepository,SupplierRepository>();
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

// Controllers endpoints
app.MapControllers();

// Simple test endpoint (IMPORTANT for debugging)
app.MapGet("/", () => "API is running!");

app.Run();