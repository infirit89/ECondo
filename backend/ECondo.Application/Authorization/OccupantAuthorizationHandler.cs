using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Authorization;

public sealed class OccupantAuthorizationHandler
    (IApplicationDbContext dbContext)
    : IResourceAuthorizationHandler<PropertyOccupant>
{
    public async Task<AccessLevel> GetAccessLevelAsync(Guid userId, Guid? resourceId, CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return AccessLevel.All;

        if (!resourceId.HasValue)
            return AccessLevel.None;

        var occupant = await dbContext
            .PropertyOccupants
            .Select(po => new
            {
                UserId = po.UserId,
                PropertyEntranceManagerId = po.Property.Entrance.ManagerId,
                PropertyId = po.PropertyId,
            })
            .FirstOrDefaultAsync
                (po => 
                        po.PropertyId == resourceId,
                    cancellationToken: cancellationToken);

        if (occupant is null)
            return AccessLevel.None;

        if (occupant.PropertyEntranceManagerId == userId)
            return AccessLevel.All;

        var isPropertyOwner = await dbContext.PropertyOccupants
            .Where(po => po.UserId == userId &&
                         po.PropertyId == occupant.PropertyId &&
                         po.OccupantType.Name == OccupantType.OwnerType)
            .AnyAsync(cancellationToken);

        if (isPropertyOwner)
            return AccessLevel.All;

        return occupant.UserId == userId ? AccessLevel.Read : AccessLevel.None;
    }

    public async Task<IQueryable<PropertyOccupant>> ApplyDataFilterAsync(IQueryable<PropertyOccupant> query, Guid userId, CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return query;

        return query.Where(po =>
            po.Property.Entrance.ManagerId == userId || // User manages the entrance
            po.Property.PropertyOccupants.Any(po2 => 
                po2.UserId == userId && po2.OccupantType.Name == OccupantType.OwnerType) || // User owns the property
            po.UserId == userId // User's own occupancy record
        );
    }
}
