using ECondo.Application.Services;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions;

internal class ProvinceSeeder { }
internal static class ProvinceSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedProvinces
        (this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var logger = services.GetRequiredService<ILogger<ProvinceSeeder>>();

        try
        {
            foreach (var province in ProvinceSeedData.Provinces)
            {
                var dbProvince = (await unitOfWork.ProvinceRepository.GetAsync(x => x.Name == province)).FirstOrDefault();
                if (dbProvince is null)
                {
                    await unitOfWork.ProvinceRepository.InsertAsync(new()
                    {
                        Name = province,
                    });
                }
            }

            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
        }

        return appBuilder;
    }
}

