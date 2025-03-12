using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public class UserLogin : IdentityUserLogin<Guid>
{
    public User User { get; set; } = null!;
}
