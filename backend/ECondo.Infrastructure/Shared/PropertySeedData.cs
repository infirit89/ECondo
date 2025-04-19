using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Shared;

internal static class PropertySeedData
{
    public static readonly Property ApartmentProperty = new Property
    {
        Id = Guid.Parse("0159e59f-c13b-4f99-b942-d2f63d4d0943"),
        EntranceId = EntranceSeedData.Entrance1.Id,
        Floor = 1,
        Number = 1,
        PropertyTypeId = PropertyTypeSeedData.ApartmentType.Id,
        BuiltArea = 10,
        IdealParts = 2,
    };

    public static readonly Property StudioProperty = new Property
    {
        Id = Guid.Parse("3393183e-f700-4bf3-84bd-560ad68e8ed2"),
        EntranceId = EntranceSeedData.Entrance1.Id,
        Floor = 1,
        Number = 2,
        PropertyTypeId = PropertyTypeSeedData.StudioType.Id,
        BuiltArea = 20,
        IdealParts = 4,
    };

    public static readonly Property OfficeProperty = new Property
    {
        Id = Guid.Parse("652b9c41-6f2a-42a4-90aa-ea788dba4a78"),
        EntranceId = EntranceSeedData.Entrance2.Id,
        Floor = 1,
        Number = 1,
        PropertyTypeId = PropertyTypeSeedData.OfficeType.Id,
        BuiltArea = 15,
        IdealParts = 3,
    };

    public static readonly IEnumerable<Property> Properties =
    [
        ApartmentProperty,
        StudioProperty,
        OfficeProperty,
    ];
}