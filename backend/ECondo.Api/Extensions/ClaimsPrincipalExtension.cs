using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECondo.Api.Extensions;
public static class ClaimsPrincipalExtension
{
    public static Claim? GetEmailClaim(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity claimsIdentity)
            return null;

        return claimsIdentity.FindFirst(x => x.Type.Contains(JwtRegisteredClaimNames.Email));
    }

    public static Claim? GetGivenNameClaim(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity claimsIdentity)
            return null;

        return claimsIdentity.FindFirst(x => x.Type.Contains(JwtRegisteredClaimNames.GivenName));
    }
}
