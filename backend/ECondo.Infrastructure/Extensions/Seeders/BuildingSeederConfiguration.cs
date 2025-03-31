using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;


internal class BuildingSeeder
{
}

internal static class BuildingSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedBuildings(this IApplicationBuilder appBuilder)
    {

        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var logger = services.GetRequiredService<ILogger<BuildingSeeder>>();

        try
        {
            Building? building = await unitOfWork.Buildings.GetByIdAsync(BuildingSeedData.TestBuildingId);

            if (building != null)
                return appBuilder;

            Province province = (await unitOfWork.Provinces.GetAsync(p => p.Name == BuildingSeedData.TestBuildingProvinceName)).First();

            building = new()
            {
                Id = BuildingSeedData.TestBuildingId,
                Name = BuildingSeedData.TestBuildingName,
                Province = province,
                Municipality = BuildingSeedData.TestBuildingMunicipality,
                SettlementPlace = BuildingSeedData.TestBuildingSettlementPlace,
                Neighborhood = BuildingSeedData.TestBuildingNeighborhood,
                PostalCode = BuildingSeedData.TestBuildingPostalCode,
                Street = BuildingSeedData.TestBuildingStreet,
                StreetNumber = BuildingSeedData.TestBuildingStreetNumber,
                BuildingNumber = BuildingSeedData.TestBuildingBuildingNumber,
            };

            await unitOfWork.Buildings.InsertAsync(building);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
        }


        return appBuilder;
    }
}

