using ECondo.Application.Shared;

namespace ECondo.Application.Authorization;

public interface IAuthorizationHandler
{
    bool CanHandle(string permission);

    Task<bool> HandleAsync(Guid userId, string permission, ResourceContext? resource,
        CancellationToken cancellationToken = default);
}
