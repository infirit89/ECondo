using ECondo.Application.Shared;
using ECondo.Domain.Permissions;

namespace ECondo.Application.Policies.Bill;

public interface ICanAddBill : IAuthRequirementResource
{
    Guid BuildingId { get; init; }
    string EntranceNumber { get; init; }
    string IAuthRequirement.Permission => BillPermissions.Create;

    ResourceContext IAuthRequirementResource.Resource => new() { Id = BuildingId, Additional = EntranceNumber };
}