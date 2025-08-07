using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Users;
public static class UserErrors
{
    public static Error InvalidRefreshToken() =>
        Error.NotFound("Users.InvalidRefreshToken", "The provided refresh token is not valid");

    public static Error InvalidUser(string username) =>
        Error.NotFound("Users.NotFound", $"The user with the Username = '{username}' was not found");

    public static Error InvalidUser() => 
        Error.NotFound("Users.NotFound", "The user was not found");

    public static Error InvalidUser(Guid id) =>
        Error.NotFound("Users.NotFound", $"The user with Id = '{id}' was not found");

    public static Error EmailNotConfirmed() => 
        Error.Conflict("Users.NotConfirmed", "The user's email is not confirmed");

    public static Error Forbidden(Guid id) =>
        Error.Forbidden(
            "Users.Forbidden", 
            $"The user with id '{id}' doesn't have the right access");
}
