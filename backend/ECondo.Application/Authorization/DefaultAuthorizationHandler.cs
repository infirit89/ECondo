using ECondo.Domain.Authorization;

namespace ECondo.Application.Authorization;

public sealed class DefaultAuthorizationHandler<T> : IResourceAuthorizationHandler<T> where T : class
{
    public Task<IQueryable<T>> ApplyDataFilterAsync(IQueryable<T> query, Guid userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(query);
    }

    public Task<AccessLevel> GetAccessLevelAsync(Guid userId, Guid? resourceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(AccessLevel.None);
    }
}
