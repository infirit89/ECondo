using ECondo.Application.Repositories;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class OccupantSeeder;

internal static class OccupantSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedOccupants(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<OccupantSeeder>>();
        
        using (logger.BeginScope("Basic tenant occupations creation"))
        {
            try
            {
                logger.LogInformation("Creating basic tenant occupations");
                await dbContext.PropertyOccupants.AddRangeAsync(OccupantSeedData.BasicTenantOccupants);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Basic tenant occupations creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        
        using (logger.BeginScope("Basic owner occupations creation"))
        {
            try
            {
                logger.LogInformation("Creating basic owner occupations");
                await dbContext.PropertyOccupants.AddRangeAsync(OccupantSeedData.BasicOwnerOccupants);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Basic owner occupations creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        return appBuilder;
    }
}