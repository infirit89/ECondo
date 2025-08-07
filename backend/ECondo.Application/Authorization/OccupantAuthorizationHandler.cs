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

        var authData = await dbContext
            .Properties
            .AsNoTracking()
            .Where(p => p.Id == resourceId)
            .Select(p => new
            {
                IsManager = p.Entrance.ManagerId == userId,
                IsOwner = p.PropertyOccupants.Any(po =>
                    po.UserId == userId && po.OccupantType.Name == OccupantType.OwnerType),
                IsOccupant = p.PropertyOccupants.Any(po =>
                    po.UserId == userId)
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (authData is null)
            return AccessLevel.None;

        if (authData.IsManager)
            return AccessLevel.All;
        
        if (authData.IsOwner)
            return AccessLevel.All;

        return authData.IsOccupant ? AccessLevel.Read : AccessLevel.None;
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
