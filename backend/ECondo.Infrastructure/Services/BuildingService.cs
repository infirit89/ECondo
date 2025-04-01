using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Infrastructure.Services;

internal sealed class BuildingService(
    ECondoDbContext context) : IBuildingService
{
    public Task<BuildingResult[]> GetBuildingsForUser(Guid userId)
    {
        return context.Entrances
            .Include(e => e.Building)
            .ThenInclude(b => b.Province)
            .Where(e => e.ManagerId == userId)
            .Select(e => new BuildingResult(
                e.BuildingId,
                e.Building.Name,
                e.Building.Province.Name,
                e.Building.Municipality,
                e.Building.SettlementPlace,
                e.Building.Neighborhood,
                e.Building.PostalCode,
                e.Building.Street,
                e.Building.StreetNumber,
                e.Building.BuildingNumber,
                e.Number)
            ).ToArrayAsync();
    }
}