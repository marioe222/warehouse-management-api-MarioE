using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Behaviors;
using Warehouse.Application.Mapping;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Domain.Interface;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Presentation.Filters;
using Warehouse.Presentation.Middleware;
using Warehouse.Presentation.Swagger;


var builder = WebApplication.CreateBuilder(args);


AppContext.SetSwitch(
    "Npgsql.EnableLegacyTimestampBehavior",
    true
);


// Controllers + Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ActionLoggingFilter>();
})
.AddDataAnnotationsLocalization();


builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<ActionLoggingFilter>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Database
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly
    );

    cfg.AddOpenBehavior(
        typeof(ValidationBehavior<,>)
    );
});


// FluentValidation
builder.Services.AddValidatorsFromAssembly(
    typeof(CreateProductCommand).Assembly
);


// Localization
// IMPORTANT: no ResourcesPath here. SharedResources.cs/.resx already live in
// the "Resources" folder, so the class namespace is already
// Warehouse.Presentation.Resources. Setting ResourcesPath = "Resources"
// makes the localizer look for Warehouse.Presentation.Resources.Resources.SharedResources
// (doubled path) which never matches the actual embedded resource name.
builder.Services.AddLocalization();


var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
    new CultureInfo("ar")
};


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");

    options.SupportedCultures = supportedCultures;

    options.SupportedUICultures = supportedCultures;
});


// Dependency Injection - Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();


// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<LocalizationHeaderOperationFilter>();
});


var app = builder.Build();


// Localization
app.UseRequestLocalization();


// Custom Middleware
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseExceptionHandling();


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