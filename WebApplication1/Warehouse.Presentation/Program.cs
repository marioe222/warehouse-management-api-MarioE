using System.Globalization;
using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Warehouse.Application.Behaviors;
using Warehouse.Application.Mapping;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Infrastructure;
using Warehouse.Presentation.Filters;
using Warehouse.Presentation.Jobs;
using Warehouse.Presentation.Middleware;
using Warehouse.Presentation.Swagger;


var builder = WebApplication.CreateBuilder(args);


// Serilog Configuration


builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            "Logs/log-.txt",
            rollingInterval: RollingInterval.Day);
});

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


// Infrastructure
// Database + Firebase + Repositories

builder.Services.AddInfrastructure(
    builder.Configuration);


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


// Firebase JWT Authentication

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var projectId =
            builder.Configuration["Firebase:ProjectId"];


        options.Authority =
            $"https://securetoken.google.com/{projectId}";


        options.Audience = projectId;


        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                RoleClaimType = "role"
            };
});


// Authorization Policies

builder.Services.AddAuthorization(options =>
{
    // Admin users:
    // Create products
    // Update products
    // Delete products
    // Upload files

    options.AddPolicy(
        "AdminPolicy",
        policy =>
        {
            policy.RequireRole("admin");
        });


    // Normal users + admins:
    // Read products
    // Read suppliers
    // Read dashboard
    // Read stock data

    options.AddPolicy(
        "UserPolicy",
        policy =>
        {
            policy.RequireRole(
                "admin",
                "user");
        });
});


// Hangfire Configuration

builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});


builder.Services.AddHangfireServer();


// Register Hangfire Job

builder.Services.AddScoped<ProductExpiryJob>();


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


// HTTPS + Authentication + Authorization

app.UseHttpsRedirection();

app.UseHangfireDashboard();


// Firebase Token Validation

app.UseAuthentication();


// Authorization Policy Check

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


// Recurring Hangfire Job

RecurringJob.AddOrUpdate<ProductExpiryJob>(
    "check-expired-products",
    job => job.CheckProductsAsync(CancellationToken.None),
    Cron.Daily);


// Run

app.Run();