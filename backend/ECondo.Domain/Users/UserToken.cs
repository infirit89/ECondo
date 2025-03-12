using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public class UserToken : IdentityUserToken<Guid>
{
    public User User { get; set; } = null!;
}
