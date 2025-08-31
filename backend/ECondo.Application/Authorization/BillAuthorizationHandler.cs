using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Authorization;
using ECondo.Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Authorization;

public sealed class BillAuthorizationHandler
    (IApplicationDbContext dbContext)
    : IResourceAuthorizationHandler<Bill>
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
            .Bills
            .AsNoTracking()
            .Where(e => e.Id == resourceId)
            .Select(b => new
            {
                IsManager = b.Entrance.ManagerId == userId,
                IsOccupant = b.Entrance.Properties.Any(p => p.PropertyOccupants.Any(po => po.UserId == userId))
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (authData is null)
            return AccessLevel.None;
        
        if (authData.IsManager)
            return AccessLevel.All;

        return authData.IsOccupant ? AccessLevel.Read : AccessLevel.None;
    }

    public async Task<IQueryable<Bill>> ApplyDataFilterAsync(IQueryable<Bill> query, Guid userId, CancellationToken cancellationToken = default)
    {
        if (await dbContext
                .UserRoles
                .IsAdminAsync(userId, cancellationToken))
            return query;

        return query.Where(b =>
                b.Entrance.ManagerId == userId ||
                b.Entrance.Properties.Any(p => p.PropertyOccupants.Any(po => po.UserId == userId)));
    }
}
