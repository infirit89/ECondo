using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Data;

internal static class OccupantSeedData
{
    private static readonly PropertyOccupant BasicTenantApartmentOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("d594e8ac-b4d0-4d85-a9d4-4fe944157d48"),
        PropertyId = PropertySeedData.ApartmentProperty.Id,
        UserId = UserSeedData.BasicTenant.User.Id,
        FirstName = UserSeedData.BasicTenantProfile.FirstName,
        MiddleName = UserSeedData.BasicTenantProfile.FirstName,
        LastName = UserSeedData.BasicTenantProfile.LastName,
        Email = UserSeedData.BasicTenant.User.Email,
        OccupantTypeId = OccupantTypeSeedData.ТenantType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };

    private static readonly PropertyOccupant BasicTenantStudioOccupant = new PropertyOccupant
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

    private static readonly PropertyOccupant BasicTenantOfficeOccupant = new PropertyOccupant
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

    private static readonly PropertyOccupant BasicOwnerApartmentOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("b14afc55-02b6-4eeb-a08e-6ce1d14e0308"),
        PropertyId = PropertySeedData.ApartmentProperty.Id,
        UserId = UserSeedData.BasicOwner.User.Id,
        FirstName = UserSeedData.BasicOwnerProfile.FirstName,
        MiddleName = UserSeedData.BasicOwnerProfile.FirstName,
        LastName = UserSeedData.BasicOwnerProfile.LastName,
        Email = UserSeedData.BasicOwner.User.Email,
        OccupantTypeId = OccupantTypeSeedData.OwnerType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };

    private static readonly PropertyOccupant BasicOwnerStudioOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("c323652c-6493-47bd-b2d0-60210ea7b762"),
        PropertyId = PropertySeedData.StudioProperty.Id,
        UserId = UserSeedData.BasicOwner.User.Id,
        FirstName = UserSeedData.BasicOwnerProfile.FirstName,
        MiddleName = UserSeedData.BasicOwnerProfile.FirstName,
        LastName = UserSeedData.BasicOwnerProfile.LastName,
        Email = UserSeedData.BasicOwner.User.Email,
        OccupantTypeId = OccupantTypeSeedData.OwnerType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };

    private static readonly PropertyOccupant BasicOwnerOfficeOccupant = new PropertyOccupant
    {
        Id = Guid.Parse("933d23ab-96a2-4ac5-8d6b-0a9f607f0920"),
        PropertyId = PropertySeedData.OfficeProperty.Id,
        UserId = UserSeedData.BasicOwner.User.Id,
        FirstName = UserSeedData.BasicOwnerProfile.FirstName,
        MiddleName = UserSeedData.BasicOwnerProfile.FirstName,
        LastName = UserSeedData.BasicOwnerProfile.LastName,
        Email = UserSeedData.BasicOwner.User.Email,
        OccupantTypeId = OccupantTypeSeedData.OwnerType.Id,
        InvitationStatus = InvitationStatus.Accepted,
    };
    
    public static readonly IEnumerable<PropertyOccupant> BasicOwnerOccupants =
    [
        BasicOwnerApartmentOccupant,
        BasicOwnerStudioOccupant,
        BasicOwnerOfficeOccupant,
    ];

}