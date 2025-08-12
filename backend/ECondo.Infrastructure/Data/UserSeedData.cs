using ECondo.Domain.Profiles;
using ECondo.Domain.Users;

namespace ECondo.Infrastructure.Data;

internal static class UserSeedData
{
    internal record UserSeedInfo(User User, string Password, string[] Roles);

    public static readonly UserSeedInfo BasicUser = new(
        new User
        {
            Id = Guid.Parse("83bc8baa-d4c7-44ca-9c3f-19679d0b78bf"),
            Email = "testUser@mail.com",
            UserName = "testUser@mail.com",
            PhoneNumber = "0881231231",
            EmailConfirmed = true,
        },
        Password: "testPass@T1", []
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
        Password: "testPass@T1", []
    );
    
    public static readonly UserSeedInfo BasicOwner = new(
        new User
        {
            Id = Guid.Parse("d45bcc26-77c9-4ea4-a852-b2c35f7e11dc"),
            Email = "testOwner@mail.com",
            UserName = "testOwner@mail.com",
            PhoneNumber = "0881231231",
            EmailConfirmed = true,
        },
        Password: "testPass@T1", []
    );
    
    public static readonly UserSeedInfo BasicAdmin = new(
        new User
        {
            Id = Guid.Parse("23fd9982-39e8-45b5-8b72-22e7db8f16b6"),
            Email = "testAdmin@mail.com",
            UserName = "testAdmin@mail.com",
            PhoneNumber = "0881231231",
            EmailConfirmed = true,
        },
        Password: "testPass@T1", [Role.Admin]
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
    
    public static readonly ProfileDetails BasicOwnerProfile =
        new ProfileDetails
        {
            FirstName = "Test",
            MiddleName = "Basic",
            LastName = "Owner",
            UserId = BasicOwner.User.Id,
        };
    
    public static readonly ProfileDetails BasicAdminProfile =
        new ProfileDetails
        {
            FirstName = "Test",
            MiddleName = "Basic",
            LastName = "Admin",
            UserId = BasicAdmin.User.Id,
        };

    public static readonly IEnumerable<UserSeedInfo> Users =
    [
        BasicUser,
        BasicTenant,
        BasicOwner,
        BasicAdmin,
    ];

    public static readonly IEnumerable<ProfileDetails> Profiles =
    [
        BasicUserProfile,
        BasicTenantProfile,
        BasicOwnerProfile,
        BasicAdminProfile,
    ];
}
