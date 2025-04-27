using ECondo.Application.Repositories;
using ECondo.Domain.Users;
using ECondo.Infrastructure.Data;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class RoleSeeder;

internal static class RoleSeederConfiguration
{
    public static async Task SeedRoles(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var roleManager = services
            .GetRequiredService<RoleManager<Role>>();
        var logger = services
            .GetRequiredService<ILogger<RoleSeeder>>();

        var dbContext = services
            .GetRequiredService<IApplicationDbContext>();
        
        using (logger.BeginScope("Role creation"))
        {
            foreach (var role in RoleSeedData.Roles)
            {
                try
                {
                    logger.LogInformation($"Creating Role '{role.Name}'");
                    var roleRes = await roleManager
                        .CreateAsync(role);

                    if (!roleRes.Succeeded)
                    {
                        logger.LogIdentityErrors(roleRes.Errors);
                        continue;
                    }

                    logger.LogInformation("Role successfully created");
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                }
            }

            logger.LogInformation("Roles created");
        }

    }
}