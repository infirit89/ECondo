using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Occupant;

public interface ICanAddOccupant : IAuthRequirementResource
{
    public Guid PropertyId { get; init; }
    public string OccupantType { get; init; }
    
    string IAuthRequirement.Permission => OccupantPermissions.Add;

    ResourceContext? IAuthRequirementResource.Resource => new() { Id = PropertyId, Additional = OccupantType };
}
