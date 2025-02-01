using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ECondo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Infrastructure.Extensions;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        string assemblyName = currentAssembly.GetName().Name ?? throw new NullReferenceException("Infrastructure assembly Name was null");

        string dbConnectionString = configuration.GetConnectionString("ECondo") 
                                    ?? throw new ArgumentOutOfRangeException("Failed to find connection string \"ECondo\"");
        services.AddDbContext<ECondoDbContext>(options =>
        {
            options
                .UseSqlServer(
                    dbConnectionString, 
                    x => x.MigrationsAssembly(assemblyName));
        });

        return services;
    }
}
