using ECondo.Application.Data.Occupant;
using ECondo.Application.Data.Property;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.SharedKernel.Collections;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetInEntrance;

internal sealed class GetPropertiesInEntranceQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPropertiesInEntranceQuery, PagedList<PropertyOccupantResult>>
{
    public async Task<Result<PagedList<PropertyOccupantResult>, Error>> Handle(GetPropertiesInEntranceQuery request, CancellationToken cancellationToken)
    {
        var properties = await dbContext.Entrances
            .Include(e => e.Properties)
                .ThenInclude(p => p.PropertyType)
            .Include(e => e.Properties)
                .ThenInclude(p => p.PropertyOccupants)
            .AsNoTracking()
            .Where(e =>
                e.Id == request.EntranceId &&
                e.ManagerId == userContext.UserId)
            .SelectMany(e =>
                e.Properties
                    .Select(p =>
                        new PropertyOccupantResult(
                            new PropertyResult(
                            p.Id,
                            p.Floor,
                            p.Number,
                            p.PropertyType.Name,
                            p.BuiltArea,
                            p.IdealParts),
                            p.PropertyOccupants.Take(5).Select(po => new BriefOccupantResult(po.FirstName, po.LastName)),
                            Math.Max(p.PropertyOccupants.Count - 5, 0))))
            .ToPagedListAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<PropertyOccupantResult>, Error>.Ok(properties);

    }
}
