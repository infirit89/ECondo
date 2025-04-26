using System.Collections;
using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Shared;

internal static class OccupantSeedData
{
    public static readonly PropertyOccupant BasicTenantApartmentOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("d594e8ac-b4d0-4d85-a9d4-4fe944157d48"),
        PropertyId = PropertySeedData.ApartmentProperty.Id,
        UserId = UserSeedData.BasicTenant.User.Id,
        FirstName = UserSeedData.BasicTenantProfile.FirstName,
        MiddleName = UserSeedData.BasicTenantProfile.FirstName,
        LastName = UserSeedData.BasicTenantProfile.LastName,
        Email = UserSeedData.BasicTenant.User.Email,
        OccupantTypeId = OccupantTypeSeedData.OwnerType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };
    
    public static readonly PropertyOccupant BasicTenantStudioOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("92fb9a2a-2df4-4eb0-878e-b216d1b4196b"),
        PropertyId = PropertySeedData.StudioProperty.Id,
        UserId = UserSeedData.BasicTenant.User.Id,
        FirstName = UserSeedData.BasicTenantProfile.FirstName,
        MiddleName = UserSeedData.BasicTenantProfile.FirstName,
        LastName = UserSeedData.BasicTenantProfile.LastName,
        Email = UserSeedData.BasicTenant.User.Email,
        OccupantTypeId = OccupantTypeSeedData.ТenantType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };
    
    public static readonly PropertyOccupant BasicTenantOfficeOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("724221fb-b21a-40da-8721-299a0299e860"),
        PropertyId = PropertySeedData.OfficeProperty.Id,
        UserId = UserSeedData.BasicTenant.User.Id,
        FirstName = UserSeedData.BasicTenantProfile.FirstName,
        MiddleName = UserSeedData.BasicTenantProfile.FirstName,
        LastName = UserSeedData.BasicTenantProfile.LastName,
        Email = UserSeedData.BasicTenant.User.Email,
        OccupantTypeId = OccupantTypeSeedData.UserType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };

    public static readonly IEnumerable<PropertyOccupant> BasicTenantOccupants =
    [
        BasicTenantApartmentOccupant,
        BasicTenantStudioOccupant,
        BasicTenantOfficeOccupant,
    ];
}