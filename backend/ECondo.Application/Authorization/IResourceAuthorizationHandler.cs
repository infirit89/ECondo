using ECondo.Domain.Authorization;

namespace ECondo.Application.Authorization;

public interface IResourceAuthorizationHandler<T> where T : class
{
    Task<AccessLevel> GetAccessLevelAsync(Guid userId,
        Guid? resourceId,
        CancellationToken cancellationToken = default);

    Task<IQueryable<T>> ApplyDataFilterAsync(IQueryable<T> query,
        Guid userId,
        CancellationToken cancellationToken = default);
}