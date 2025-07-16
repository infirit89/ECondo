using ECondo.Application.Authorization;
using ECondo.Application.Shared;

namespace ECondo.Infrastructure.Authorization;

internal class PropertyAuthorizationHandler : IAuthorizationHandler
{
    public bool CanHandle(string permission)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HandleAsync(Guid userId, string permission, ResourceContext? resource,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}