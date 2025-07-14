using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Property;

public interface ICanSeeProperty : IAuthRequirementResource
{
    public Guid PropertyId { get; init; }
    
    string IAuthRequirement.Permission => PropertyPermissions.View;

    ResourceContext IAuthRequirementResource.Resource => new() { Id = PropertyId };
}
