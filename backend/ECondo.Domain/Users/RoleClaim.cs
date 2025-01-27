using System.Security.Claims;

namespace ECondo.Domain.Users;

public sealed class RoleClaim
{
    public int Id { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }

    public Claim ToClaim() => new Claim(ClaimType!, ClaimValue!);
    public void InitializeFromClaim(Claim claim)
    {
        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}