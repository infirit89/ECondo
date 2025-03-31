using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECondo.Domain.Users;

namespace ECondo.Infrastructure.Extensions;
public static class ClaimsPrincipalExtension
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity claimsIdentity)
            return null;

        Claim? claim = claimsIdentity
            .FindFirst(x => x.Type.Contains(JwtRegisteredClaimNames.Email));

        return claim?.Value;
    }

    public static string? GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity claimsIdentity)
            return null;

        Claim? claim = claimsIdentity
            .FindFirst(x => x.Type.Contains(JwtRegisteredClaimNames.GivenName));

        return claim?.Value;
    }

    public static Guid? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        string? userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is null ? null : Guid.Parse(userId);
    }

    //public static UserContext? GetUserContext(this ClaimsPrincipal claimsPrincipal)
    //{
    //    string? email = claimsPrincipal.GetEmail();
    //    string? username = claimsPrincipal.GetUsername();
    //    Guid? userId = claimsPrincipal.GetId();

    //    if (email is null || username is null || userId is null)
    //        return null;

    //    return new UserContext((Guid)userId, email, username);
    //}
}
