using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Property;

public interface ICanEditProperty : IAuthRequirementResource
{
    public Guid PropertyId { get; init; }
    
    string IAuthRequirement.Permission => PropertyPermissions.Edit;
    ResourceContext IAuthRequirementResource.Resource => new() { Id = PropertyId };
}
