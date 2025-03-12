using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}