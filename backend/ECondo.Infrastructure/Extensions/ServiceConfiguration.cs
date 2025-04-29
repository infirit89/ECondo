using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Infrastructure.Repositories;
using ECondo.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace ECondo.Infrastructure.Extensions;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddInfrastructureConnections(configuration);
        services.ConfigureInfrastructureSettings(configuration);
        services.AddIdentityConfiguration(configuration);

        services.AddOptions();
        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(options =>
        {
            options.ApiToken = configuration["ResendSettings:ApiKey"]!;
        });
        services.AddTransient<IResend, ResendClient>();

        services.AddScoped<ICacheRepository, CacheRepository>();

        services.AddHttpContextAccessor();

        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddSingleton<IEmailTemplateService, HtmlEmailTemplateService>();
        services.AddScoped<IEmailService, MailService>();
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<IStripeService, StripeService>();

        services.AddHostedService<RecurringBillBackgroundService>();
        
        services.AddHealthChecks();

        services.AddSignalR();

        return services;
    }
}
    