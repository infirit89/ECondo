using ECondo.Application.Repositories;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class OccupantTypeSeeder;

internal static class OccupantTypeSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedOccupantTypes(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<OccupantTypeSeeder>>();

        using (logger.BeginScope("Occupant types creation"))
        {
            try
            {
                logger.LogInformation("Creating occupant types");
                await dbContext
                    .OccupantTypes
                    .AddRangeAsync(
                        OccupantTypeSeedData.OccupantTypes);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Occupant types creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        return appBuilder;
    }
}