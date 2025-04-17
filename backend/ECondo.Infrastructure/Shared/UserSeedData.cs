using ECondo.Application.Commands.Identity.Register;
using ECondo.Application.Commands.Profiles.Create;

namespace ECondo.Infrastructure.Shared;

internal static class UserSeedData
{
    public static readonly RegisterCommand BasicUser = 
        new("testUser@mail.com", 
            "testUser@mail.com", 
            "testPass@T1", 
            "", 
            true);

    public static readonly CreateProfileCommand BasicUserProfile =
        new("testUser@mail.com", 
            "Test", 
            "Basic", 
            "User", 
            "0881231231");

    public static readonly RegisterCommand BasicTenant =
        new("testTenant@mail.com",
            "testTenant@mail.com",
            "testPass@T1",
            "",
            true);

    public static readonly CreateProfileCommand BasicTenantProfile =
        new("testTenant@mail.com",
            "Test",
            "Basic",
            "Tenant",
            "0881231231");

    public static readonly RegisterCommand[] Users =
    [
        BasicUser,
        BasicTenant,
    ];

    public static readonly CreateProfileCommand[] Profiles =
    [
        BasicUserProfile,
        BasicTenantProfile,
    ];
}
