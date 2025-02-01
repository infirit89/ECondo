using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECondo.Application.Extensions;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(currentAssembly));

        return services;
    }
}
