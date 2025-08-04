using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Authorization;

public sealed class EntranceAuthorizationHandler
    (IApplicationDbContext dbContext) : 
    IResourceAuthorizationHandler<Entrance>
{
    public async Task<AccessLevel> GetAccessLevelAsync(Guid userId, 
        Guid? resourceId, 
        CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return AccessLevel.All;

        if (!resourceId.HasValue)
            return AccessLevel.Read;
        
        var isManager = await dbContext
            .Entrances
            .AsNoTracking()
            .AnyAsync(e =>
                    e.Id == resourceId &&
                    e.ManagerId == userId,
                cancellationToken: cancellationToken);

        if (isManager)
            return AccessLevel.All;

        return AccessLevel.None;
    }

    public async Task<IQueryable<Entrance>> ApplyDataFilterAsync(
        IQueryable<Entrance> query, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return query;

        return query.Where(e =>
            e.ManagerId == userId);
    }
}