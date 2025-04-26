using ECondo.Application.Data;
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
        var propertyQuery = dbContext.Entrances
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
                            p.IdealParts)));

        var propertyCount = await propertyQuery.CountAsync(cancellationToken: cancellationToken);

        var properties = await propertyQuery
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<PagedList<PropertyOccupantResult>, Error>.Ok(
            new PagedList<PropertyOccupantResult>(
                properties,
                propertyCount,
                request.Page,
                request.PageSize));

    }
}
