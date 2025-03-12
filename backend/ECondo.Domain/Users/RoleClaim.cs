using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public class RoleClaim : IdentityRoleClaim<Guid>
{
    public Role Role { get; set; } = null!;
}

