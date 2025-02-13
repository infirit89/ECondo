using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Infrastructure.Repositories;
using ECondo.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Infrastructure.Extensions;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddInfrastructureConnections(configuration);
        services.ConfigureInfrastructureSettings(configuration);
        services.AddIdentityConfiguration(configuration);

        services.AddScoped<IAuthTokenService, AuthTokenService>();

        services.AddScoped<ICacheRepository, CacheRepository>();

        services.AddSignalR();

        return services;
    }
}
    