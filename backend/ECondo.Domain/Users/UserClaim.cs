using System.Security.Claims;

namespace ECondo.Domain.Users;

public sealed class UserClaim
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }

    public Claim ToClaim() => new Claim(ClaimType!, ClaimValue!);
    public void InitializeFromClaim(Claim claim)
    {
        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}