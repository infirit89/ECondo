using ECondo.Application.Data;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.SharedKernel.Collections;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Buildings.GetAll;

internal sealed class GetAllBuildingsQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetAllBuildingsQuery, PagedList<BuildingResult>>
{
    public async Task<Result<PagedList<BuildingResult>, Error>> Handle(
        GetAllBuildingsQuery request, 
        CancellationToken cancellationToken)
    {
        var buildings = await dbContext.Entrances
            .AsNoTracking()
            .Include(e => e.Building)
            .ThenInclude(b => b.Province)
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
            ).ToPagedListAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken: cancellationToken);
        return Result<PagedList<BuildingResult>, Error>.Ok(buildings);
    }
}