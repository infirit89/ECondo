using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Extensions.Seeders;

// NOTE: here for in order for ILogger to function properly
internal class BuildingSeeder;

internal static class BuildingSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedBuildings(this IApplicationBuilder appBuilder)
    {

        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var unitOfWork = services.GetRequiredService<IApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<BuildingSeeder>>();

        try
        {
            Building? building = await unitOfWork
                .Buildings
                .FirstOrDefaultAsync(b => 
                    b.Id == BuildingSeedData.TestBuildingId);

            if (building != null)
                return appBuilder;

            Province province = await unitOfWork
                .Provinces
                .FirstAsync(p => 
                    p.Name == 
                    BuildingSeedData.TestBuildingProvinceName);

            building = new Building
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

            await unitOfWork.Buildings.AddAsync(building);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
        }


        return appBuilder;
    }
}

