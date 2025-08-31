using ECondo.Application.Repositories;
using ECondo.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class PropertySeeder;

internal static class PropertySeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedProperties(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<PropertySeeder>>();

        using (logger.BeginScope("Property creation"))
        {
            try
            {
                logger.LogInformation("Creating properties");
                await dbContext.Properties.AddRangeAsync(PropertySeedData.Properties);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Property creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        return appBuilder;
    }
}