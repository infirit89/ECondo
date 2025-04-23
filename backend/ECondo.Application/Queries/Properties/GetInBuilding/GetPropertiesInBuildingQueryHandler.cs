using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetInBuilding;

internal sealed class GetPropertiesInBuildingQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPropertiesInBuildingQuery, PagedList<BriefPropertyResult>>
{
    public async Task<Result<PagedList<BriefPropertyResult>, Error>> Handle(GetPropertiesInBuildingQuery request, CancellationToken cancellationToken)
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
                        new BriefPropertyResult(
                            p.Id,
                            p.Floor,
                            p.Number,
                            p.PropertyType.Name)));

        var propertyCount = await propertyQuery.CountAsync(cancellationToken: cancellationToken);

        var properties = await propertyQuery
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<PagedList<BriefPropertyResult>, Error>.Ok(
            new PagedList<BriefPropertyResult>(
                properties,
                propertyCount,
                request.Page,
                request.PageSize));

    }
}
