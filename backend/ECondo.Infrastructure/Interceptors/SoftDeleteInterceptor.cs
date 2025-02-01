using ECondo.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECondo.Infrastructure.Interceptors;

internal sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        IEnumerable<EntityEntry<ISoftDeletable>> deletedEntities =
            eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDeletable> deletedEntity in deletedEntities)
        {
            deletedEntity.State = EntityState.Modified;
            deletedEntity.Entity.Delete();
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
