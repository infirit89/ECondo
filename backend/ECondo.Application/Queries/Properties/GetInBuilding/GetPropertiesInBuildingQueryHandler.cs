using ECondo.Application.Data;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetInBuilding;

internal sealed class GetPropertiesInBuildingQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPropertiesInBuildingQuery, PagedList<PropertyOccupantResult>>
{
    public async Task<Result<PagedList<PropertyOccupantResult>, Error>> Handle(GetPropertiesInBuildingQuery request, CancellationToken cancellationToken)
    {
        var properties = await dbContext.Entrances
            .Include(e => e.Properties)
            .ThenInclude(p => p.PropertyType)
            .AsNoTracking()
            .Where(e =>
                e.BuildingId == request.BuildingId &&
                e.Number == request.EntranceNumber &&
                e.ManagerId == userContext.UserId)
            .SelectMany(e =>
                e.Properties
                    .Select(p =>
                        new PropertyOccupantResult(
                            p.Id,
                            p.Floor,
                            p.Number,
                            p.PropertyType.Name,
                            p.BuiltArea,
                            p.IdealParts)))
            .ToPagedListAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<PropertyOccupantResult>, Error>.Ok(properties);

    }
}
