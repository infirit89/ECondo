using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Data;

internal static class OccupantTypeSeedData
{
    public static readonly OccupantType ТenantType = new OccupantType
    {
        Id = Guid.Parse("bfe5e4d5-3720-4205-b02c-8ca416cf9aa2"),
        Name = "Наемател",
    };

    public static readonly OccupantType OwnerType = new OccupantType
    {
        Id = Guid.Parse("cd0a8523-212f-4ed7-b6cf-cc779f4aabde"),
        Name = "Собственик",
    };

    public static readonly OccupantType UserType = new OccupantType
    {
        Id = Guid.Parse("566f8ad5-4f03-4662-b690-3d88e929f7e9"),
        Name = "Ползвател",
    };

    public static readonly OccupantType RepresentativeType = new OccupantType
    {
        Id = Guid.Parse("67cbf878-01b6-4800-87c9-a4ced25aad35"),
        Name = "Представител",
    };

    public static readonly IEnumerable<OccupantType> OccupantTypes =
    [
        ТenantType,
        OwnerType,
        UserType,
        RepresentativeType,
    ];
}