using ECondo.Application.Repositories;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class PropertyTypeSeeder;

internal static class PropertyTypeSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedPropertyTypes(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<PropertyTypeSeeder>>();

        using (logger.BeginScope("Property types creation"))
        {
            try
            {
                logger.LogInformation("Creating property types");
                await dbContext.PropertyTypes.AddRangeAsync(PropertyTypeSeedData.PropertyTypes);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Property types creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        return appBuilder;
    }
}