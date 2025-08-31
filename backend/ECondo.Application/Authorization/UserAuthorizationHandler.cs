using ECondo.Domain.Authorization;
using ECondo.Domain.Users;

namespace ECondo.Application.Authorization;

internal sealed class UserAuthorizationHandler : IResourceAuthorizationHandler<User>
{
    public Task<AccessLevel> GetAccessLevelAsync(Guid userId, Guid? resourceId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<User>> ApplyDataFilterAsync(IQueryable<User> query, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}