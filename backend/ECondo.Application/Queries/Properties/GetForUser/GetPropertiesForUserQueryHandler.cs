using ECondo.Application.Data;
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
        var propertyQuery = dbContext.Properties
            .Include(p => p.PropertyType)
            .AsNoTracking()
            .Where(p =>
                p.PropertyOccupants.Any(po => po.UserId == userContext.UserId))
            .Select(p =>
                new PropertyOccupantResult(
                    p.Id,
                    p.Floor,
                    p.Number,
                    p.PropertyType.Name,
                    p.BuiltArea,
                    p.IdealParts));

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