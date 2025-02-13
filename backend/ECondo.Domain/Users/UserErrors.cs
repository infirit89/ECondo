using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public static class UserErrors
{
    public static IdentityError InvalidRefreshToken() => new()
    {
        Code = "Users.InvalidRefreshToken",
        Description = "The provided refresh token is not valid",
    };

    public static IdentityError InvalidUser(string username) => new()
    {
        Code = "Users.NotFound",
        Description = $"The user with the Username = '{username}' was not found"
    };

    public static IdentityError InvalidUser() => new()
    {
        Code = "Users.NotFound",
        Description = "The user was not found"
    };
}
