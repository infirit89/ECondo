using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Occupant;

public interface ICanEditOccupant : IAuthRequirementResource
{
    public Guid OccupantId { get; init; }
    public string Type { get; init; }
    
    string IAuthRequirement.Permission => OccupantPermissions.Edit;

    ResourceContext IAuthRequirementResource.Resource => new() { Id = OccupantId, Additional = Type };
}
