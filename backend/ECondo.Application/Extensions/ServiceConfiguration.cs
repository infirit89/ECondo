using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ECondo.Application.Behaviours;
using FluentValidation;

namespace ECondo.Application.Extensions;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(currentAssembly);

            configuration.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            configuration.AddOpenBehavior(typeof(AuthorizationPipelineBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(currentAssembly, includeInternalTypes: true);
        services.AddAuthorizersFromAssembly(currentAssembly);
        return services;
    }
}
