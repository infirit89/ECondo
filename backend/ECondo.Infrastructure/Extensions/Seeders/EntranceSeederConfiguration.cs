using ECondo.Application.Repositories;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class EntranceSeeder;

internal static class EntranceSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedEntrances(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<ProvinceSeeder>>();

        using (logger.BeginScope("Entrance creation"))
        {
            try
            {
                logger.LogInformation("Creating entrances");
                await dbContext
                    .Entrances
                    .AddRangeAsync(EntranceSeedData.Entrances);

                await dbContext.SaveChangesAsync();
                logger.LogInformation("Entrance creation completed");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        return appBuilder;
    }
}