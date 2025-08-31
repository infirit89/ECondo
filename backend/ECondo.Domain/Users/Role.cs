using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;

public class Role : IdentityRole<Guid>
{
    public HashSet<UserRole> UserRoles { get; set; } = null!;
    public HashSet<RoleClaim> RoleClaims { get; set; } = null!;

    public const string Admin = "admin";
}
