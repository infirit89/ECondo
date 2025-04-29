using ECondo.Domain.Shared;

namespace ECondo.Domain.Users;

public static class RoleErrors
{
    public static Error NotFound(Guid userId, string roleName) =>
        Error.NotFound(
            "Roles.NotFound", 
            $"The user with Id '{userId}' is not in the role '{roleName}'");
}