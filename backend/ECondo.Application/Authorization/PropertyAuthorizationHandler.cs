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

        var authData = await dbContext
            .Properties
            .Where(p => p.Id == resourceId)
            .Select(p => new
            {
                IsManager = p.Entrance.ManagerId == userId,
                IsOccupant = p.PropertyOccupants.Any(po => po.UserId == userId)
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (authData is null)
            return AccessLevel.None;
        
        if (authData.IsManager)
            return AccessLevel.All;
        
        return authData.IsOccupant ? AccessLevel.Read : AccessLevel.None;
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