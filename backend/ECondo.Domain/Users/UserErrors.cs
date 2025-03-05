using ECondo.Domain.Shared;

namespace ECondo.Domain.Users;
public static class UserErrors
{
    public static Error InvalidRefreshToken() => new()
    {
        Code = "Users.InvalidRefreshToken",
        Description = "The provided refresh token is not valid",
    };

    public static Error InvalidUser(string username) => new()
    {
        Code = "Users.NotFound",
        Description = $"The user with the Username = '{username}' was not found",
    };

    public static Error InvalidUser() => new()
    {
        Code = "Users.NotFound",
        Description = "The user was not found",
    };

    public static Error EmailNotConfirmed() => new()
    {
        Code = "Users.NotConfirmed",
        Description = "The user's email is not confirmed",
    };
}
