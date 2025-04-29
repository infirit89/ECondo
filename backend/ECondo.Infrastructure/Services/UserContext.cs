using ECondo.Application.Services;
using ECondo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace ECondo.Infrastructure.Services;

internal class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId { get; } =
        httpContextAccessor
            .HttpContext?
            .User
            .GetId() ??
        throw new ApplicationException("User context is unavailable");
}
