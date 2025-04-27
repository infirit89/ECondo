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
            configuration.AddOpenBehavior(typeof(EntranceManagerAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(AccessPropertyAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(EditPropertyAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(EditOccupantAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(AddOccupantAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(AccessTenantAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(DeleteOccupantAuthorizationPipelineBehaviour<,>));
            configuration.AddOpenBehavior(typeof(AdminAuthorizationPipelineBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(currentAssembly, includeInternalTypes: true);

        return services;
    }
}
