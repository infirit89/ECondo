using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Admin;

public interface IIsAdmin : IAuthRequirement
{
    string IAuthRequirement.Permission => AdminPermissions.Access;
}
