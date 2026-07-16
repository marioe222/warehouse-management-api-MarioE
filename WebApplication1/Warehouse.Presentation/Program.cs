using Serilog;
using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;

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



// Serilog Configuration


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Host.UseSerilog();



// PostgreSQL timestamp compatibility


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
        builder.Configuration
            .GetConnectionString("DefaultConnection")
    ));



// Redis Cache


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration
            .GetConnectionString("Redis");

    options.InstanceName = "Warehouse_";
});



// Health Checks


builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration
            .GetConnectionString("DefaultConnection")!)
    .AddRedis(
        builder.Configuration
            .GetConnectionString("Redis")!);



// Health Check UI


builder.Services
    .AddHealthChecksUI(options =>
    {
        options.AddHealthCheckEndpoint(
            "Warehouse API",
            "/health");
    })
    .AddInMemoryStorage();



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


builder.Services.AddLocalization();


var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
    new CultureInfo("ar")
};


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture =
        new RequestCulture("en");


    options.SupportedCultures =
        supportedCultures;


    options.SupportedUICultures =
        supportedCultures;
});



// Dependency Injection
// Repositories


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();



// Swagger


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<LocalizationHeaderOperationFilter>();
});



// Build Application


var app = builder.Build();



// Localization Middleware


app.UseRequestLocalization();



// Custom Middleware

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseExceptionHandling();


// Swagger


app.UseSwagger();

app.UseSwaggerUI();



// HTTPS + Authorization

app.UseHttpsRedirection();

app.UseAuthorization();



// Controllers


app.MapControllers();


// Health Check Endpoint

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter =
            UIResponseWriter.WriteHealthCheckUIResponse
    });


// Health Dashboard


app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});


// Run

app.Run();