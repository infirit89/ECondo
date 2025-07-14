using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Occupant;

public interface ICanSeeOccupants : IAuthRequirementResource
{
    public Guid PropertyId { get; init; }
    public string OccupantType { get; init; }
    
    string IAuthRequirement.Permission => OccupantPermissions.View;
    ResourceContext IAuthRequirementResource.Resource => new() { Id = PropertyId, Additional = OccupantType };
}
