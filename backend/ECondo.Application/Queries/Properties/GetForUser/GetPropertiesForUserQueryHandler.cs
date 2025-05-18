using ECondo.Application.Data;
using ECondo.Application.Data.Occupant;
using ECondo.Application.Data.Property;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetForUser;

internal sealed class GetPropertiesForUserQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPropertiesForUserQuery, PagedList<PropertyOccupantResult>>
{
    public async Task<Result<PagedList<PropertyOccupantResult>, Error>> Handle(GetPropertiesForUserQuery request, CancellationToken cancellationToken)
    {
        var properties = await dbContext.Properties
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyOccupants)
            .AsNoTracking()
            .Where(p =>
                p.PropertyOccupants.Any(po => po.UserId == userContext.UserId))
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
                    1))
            .ToPagedListAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<PropertyOccupantResult>, Error>.Ok(properties);
    }
}