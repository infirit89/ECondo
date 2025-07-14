using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.EntranceManager;

internal interface IIsEntranceManager : IAuthRequirementResource
{
    Guid BuildingId { get; init; }
    string EntranceNumber { get; init; }

    string IAuthRequirement.Permission => BuildingPermissions.Manage;
    ResourceContext IAuthRequirementResource.Resource => new() { Id = BuildingId, Additional = EntranceNumber };
}
