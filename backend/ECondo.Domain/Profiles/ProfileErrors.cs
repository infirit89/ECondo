using ECondo.Domain.Shared;

namespace ECondo.Domain.Profiles;
public static class ProfileErrors
{
    public static Error InvalidProfile(string username) => new()
    {
        Code = "Profile.NotFound",
        Description = $"The profile for user with name = '{username}' was not found",
    };

    public static Error InvalidProfile(Guid id) => new()
    {
        Code = "Profile.NotFound",
        Description = $"The profile for user with Id = '{id}' was not found",
    };
}
