﻿using ECondo.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Infrastructure.Extensions;
internal static class SettingsConfiguration
{
    public static IServiceCollection ConfigureInfrastructureSettings(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<RefreshTokenSettings>(configuration.GetSection(nameof(RefreshTokenSettings)));
        services.Configure<StripeSettings>(configuration.GetSection(nameof(StripeSettings)));
        return services;
    }
}
