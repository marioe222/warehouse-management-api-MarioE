using FluentValidation;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Application.Products.Commands.CreateProduct;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;
using Warehouse.Application.Mapping;
using Warehouse.Application.Behaviors;
using Warehouse.Presentation.Middleware;
using Warehouse.Presentation.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ActionLoggingFilter>();
});

builder.Services.AddScoped<ValidationFilter>();

builder.Services.AddScoped<ActionLoggingFilter>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );


    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>)
    );
});

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateProductCommand).Assembly
);


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
    
builder.Services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();