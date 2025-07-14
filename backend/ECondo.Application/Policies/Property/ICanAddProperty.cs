using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Property;

public interface ICanAddProperty : IAuthRequirementResource
{
    Guid BuildingId { get; init; }
    string EntranceNumber { get; init; }
    
    string IAuthRequirement.Permission => PropertyPermissions.Add;

    ResourceContext IAuthRequirementResource.Resource => new() { Id = BuildingId, Additional = EntranceNumber };
}