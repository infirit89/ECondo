using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public class UserClaim : IdentityUserClaim<Guid>
{
    public User User { get; set; } = null!;
}
