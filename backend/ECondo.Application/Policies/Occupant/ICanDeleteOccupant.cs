using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Occupant;

public interface ICanDeleteOccupant : IAuthRequirementResource
{
    public Guid OccupantId { get; init; }
    
    string IAuthRequirement.Permission => OccupantPermissions.Delete;
    ResourceContext IAuthRequirementResource.Resource => new() { Id = OccupantId };
}
