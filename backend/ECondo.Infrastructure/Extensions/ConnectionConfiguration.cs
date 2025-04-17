using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ECondo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ECondo.Application.Repositories;
using ECondo.Infrastructure.Shared;

namespace ECondo.Infrastructure.Extensions;

internal static class ConnectionConfiguration
{
    public static IServiceCollection AddInfrastructureConnections(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        string assemblyName = 
            currentAssembly.GetName().Name 
            ?? throw new NullReferenceException(
                "Infrastructure assembly Name was null");

        string dbConnectionString = 
            configuration.GetConnectionString("ECondo")
            ?? throw new ArgumentOutOfRangeException(
                Resources.DbConnectionError);
        services.AddDbContext<ECondoDbContext>(options =>
        {
            options
                .UseSqlServer(
                    dbConnectionString,
                    x => 
                        x.MigrationsAssembly(assemblyName));
        });

        services
            .AddScoped<IApplicationDbContext>(sp => 
                sp.GetRequiredService<ECondoDbContext>());

        string redisConnectionString = 
            configuration.GetConnectionString("Redis") 
            ?? throw new ArgumentOutOfRangeException(
                Resources.RedisConnectionError);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        return services;
    }
}