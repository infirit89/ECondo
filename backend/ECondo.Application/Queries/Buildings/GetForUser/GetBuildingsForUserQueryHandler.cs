using ECondo.Application.Data;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Buildings.GetForUser;

internal sealed class GetBuildingsForUserQueryHandler(
    IUserContext userContext,
    IApplicationDbContext dbContext)
    : IQueryHandler<
        GetBuildingsForUserQuery, PagedList<BuildingResult>>
{
    public async Task<Result<PagedList<BuildingResult>, Error>> 
        Handle(GetBuildingsForUserQuery request, 
            CancellationToken cancellationToken)
    {
        var buildingsQuery = dbContext.Entrances
            .AsNoTracking()
            .Include(e => e.Building)
            .ThenInclude(b => b.Province)
            .Where(e => e.ManagerId == userContext.UserId);

        if (request.BuildingName is not null)
        {
            buildingsQuery = buildingsQuery
                .Where(b => b.Building.Name.Contains(request.BuildingName));
        }

        var buildings = await buildingsQuery
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
                e.Number))
            .ToPagedListAsync(
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken);
        
        return Result<PagedList<BuildingResult>, Error>.Ok(buildings);
    }
}