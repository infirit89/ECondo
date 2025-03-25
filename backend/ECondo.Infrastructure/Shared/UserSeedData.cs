using ECondo.Application.Commands.Identity;
using ECondo.Application.Commands.Profile;

namespace ECondo.Infrastructure.Shared;

internal static class UserSeedData
{
    public static readonly RegisterCommand BasicUser = new("testUser@mail.com", "testUser@mail.com", "testPass@T1", "", true);

    public static readonly CreateProfileCommand BasicUserProfile =
        new("testUser@mail.com", "Test", "Basic", "User", "0881231231");
}
