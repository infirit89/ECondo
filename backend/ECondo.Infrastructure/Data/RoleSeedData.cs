using ECondo.Domain.Users;

namespace ECondo.Infrastructure.Data;

public static class RoleSeedData
{
    public static readonly Role AdminRole = new()
    {
        Id = Guid.Parse("0e052fa5-fedb-4ce1-a4fd-599b8725e244"),
        Name = Role.Admin,
    };

    public static readonly Role[] Roles =
    [
        AdminRole
    ];
}