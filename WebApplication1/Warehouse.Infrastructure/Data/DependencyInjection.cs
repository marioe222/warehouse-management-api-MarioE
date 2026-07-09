using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        return services;
    }
}