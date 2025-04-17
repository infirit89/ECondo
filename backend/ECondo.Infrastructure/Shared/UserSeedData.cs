using ECondo.Domain.Profiles;
using ECondo.Domain.Users;

namespace ECondo.Infrastructure.Shared;

internal static class UserSeedData
{
    internal record UserSeedInfo(User User, string Password);

    public static readonly UserSeedInfo BasicUser = new(
        new User
        {
            Id = Guid.Parse("83bc8baa-d4c7-44ca-9c3f-19679d0b78bf"),
            Email = "testUser@mail.com",
            UserName = "testUser@mail.com",
            PhoneNumber = "0881231231",
            EmailConfirmed = true,
        },
        Password: "testPass@T1"
    );


    public static readonly UserSeedInfo BasicTenant = new(
        new User
        {
            Id = Guid.Parse("769ccd71-0e9a-4a73-b1c5-976f759b8946"),
            Email = "testTenant@mail.com",
            UserName = "testTenant@mail.com",
            PhoneNumber = "0881231231",
            EmailConfirmed = true,
        },
        Password: "testPass@T1"
    );

    public static readonly ProfileDetails BasicUserProfile =
        new ProfileDetails
        {
            FirstName = "Test",
            MiddleName = "Basic",
            LastName = "User",
            UserId = BasicUser.User.Id,
        };

    public static readonly ProfileDetails BasicTenantProfile =
        new ProfileDetails
        {
            FirstName = "Test",
            MiddleName = "Basic",
            LastName = "Tenant",
            UserId = BasicTenant.User.Id,
        };

    public static readonly IEnumerable<UserSeedInfo> Users =
    [
        BasicUser,
        BasicTenant,
    ];

    public static readonly IEnumerable<ProfileDetails> Profiles =
    [
        BasicUserProfile,
        BasicTenantProfile,
    ];
}
