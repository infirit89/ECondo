using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ECondo.Domain.Users;
using ECondo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using ECondo.Domain.Shared;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        string redisConnectionString = configuration.GetConnectionString("Redis") ?? throw new ArgumentOutOfRangeException("Failed to find the connection string for Redis");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        //services.Configure<RefreshTokenSettings>(configuration.GetSection(nameof(RefreshTokenSettings)));

        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme =
                options.DefaultAuthenticateScheme =
                    options.DefaultChallengeScheme =
                        options.DefaultForbidScheme =
                            options.DefaultSignInScheme =
                                options.DefaultSignOutScheme =
                                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("Identity.Bearer", options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
            };
        });

        services.AddAuthorizationBuilder();

        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddApiEndpoints()
        .AddEntityFrameworkStores<ECondoDbContext>();

        services.AddSignalR();

        return services;
    }
}
    