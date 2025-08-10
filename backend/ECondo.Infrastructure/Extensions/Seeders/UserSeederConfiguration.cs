using ECondo.Application.Repositories;
using ECondo.Domain.Users;
using ECondo.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class UserSeeder;

internal static class UserSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedUsers(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var userManager = services
            .GetRequiredService<UserManager<User>>();
        var logger = services
            .GetRequiredService<ILogger<UserSeeder>>();

        var dbContext = services
            .GetRequiredService<IApplicationDbContext>();
        
        using (logger.BeginScope("User creation"))
        {
            foreach (var userData in UserSeedData.Users)
            {
                try
                {
                    logger.LogInformation($"Creating User '{userData.User.UserName}'");
                    var userRes = await userManager
                        .CreateAsync(userData.User, userData.Password);
                    
                    if (!userRes.Succeeded)
                    {
                        logger.LogIdentityErrors(userRes.Errors);
                        continue;
                    }

                    foreach (var role in userData.Roles)
                    {
                        var userRoleRes = await userManager.AddToRoleAsync(userData.User, role);
                        if (!userRoleRes.Succeeded)
                        {
                            logger.LogIdentityErrors(userRoleRes.Errors);
                        }
                        
                    }

                    logger.LogInformation("User successfully created");
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                }
            }

            logger.LogInformation("Users created");
        }


        using (logger.BeginScope("Profile creation"))
        {
            try
            {
                await dbContext.UserDetails.AddRangeAsync(UserSeedData.Profiles);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Profiles created successfully");
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }

        return appBuilder;
    }
}
