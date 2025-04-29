using ECondo.Application.Repositories;
using ECondo.Domain.Provinces;
using ECondo.Infrastructure.Data;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class ProvinceSeeder;
internal static class ProvinceSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedProvinces
        (this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var unitOfWork = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<ProvinceSeeder>>();

        using (logger.BeginScope("Province creation"))
        {
            try
            {
                foreach (var provinceName in ProvinceSeedData.Provinces)
                {
                    logger.LogInformation($"Creating province {provinceName}");
                    var province = await unitOfWork
                        .Provinces
                        .FirstOrDefaultAsync(x =>
                            x.Name == provinceName);
                    if (province is not null)
                        continue;

                    await unitOfWork.Provinces.AddAsync(new Province
                    {
                        Name = provinceName,
                    });
                }

                logger.LogInformation("Updating Database");
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
            }

            logger.LogInformation("Province creation completed");
        }

        return appBuilder;
    }
}

