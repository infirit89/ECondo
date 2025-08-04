using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Authorization;

public sealed class PropertyAuthorizationHandler
    (IApplicationDbContext dbContext)
    : IResourceAuthorizationHandler<Property>
{
    public async Task<AccessLevel> GetAccessLevelAsync(Guid userId, Guid? resourceId, CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return AccessLevel.All;

        if (!resourceId.HasValue)
            return AccessLevel.Read;
        
        var isManager = await dbContext
            .Properties
            .AsNoTracking()
            .AnyAsync(e =>
                    e.Id == resourceId &&
                    e.Entrance.ManagerId == userId,
                cancellationToken: cancellationToken);

        if (isManager)
            return AccessLevel.All;

        var isOccupant = await dbContext
            .PropertyOccupants
            .AsNoTracking()
            .Where(po => po.UserId == userId && po.PropertyId == resourceId)
            .AnyAsync(cancellationToken);

        return isOccupant ? AccessLevel.Read : AccessLevel.None;
    }

    public async Task<IQueryable<Property>> ApplyDataFilterAsync(IQueryable<Property> query, Guid userId, CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return query;

        return query.Where(p =>
            p.Entrance.ManagerId == userId ||
            p.PropertyOccupants.Any(po => po.UserId == userId));
    }
}