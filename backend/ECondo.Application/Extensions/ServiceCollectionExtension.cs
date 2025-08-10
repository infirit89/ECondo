using System.Reflection;
using ECondo.Application.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthorizersFromAssembly(
        this IServiceCollection services, Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var authorizerInterfaceType = typeof(IResourceAuthorizationHandler<>);
        var authorizers = assembly.DefinedTypes.Where(t =>
            t is { IsClass: true, IsAbstract: false } && 
            t != authorizerInterfaceType && 
            t.ImplementedInterfaces.Any(i =>
                i.IsGenericType && 
                i.GetGenericTypeDefinition() == authorizerInterfaceType))
            .Except([typeof(DefaultAuthorizationHandler<>)])
            .ToList();

        services.AddScoped(typeof(DefaultAuthorizationHandler<>));
        foreach (var authorizer in authorizers)
        {
            foreach (var implementedInterface in authorizer.GetInterfaces())
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementedInterface, authorizer);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementedInterface, authorizer);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementedInterface, authorizer);
                        break;
                }
            }
        }

        return services;
    }
}
