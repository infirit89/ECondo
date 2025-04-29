using ECondo.Domain.Buildings;

namespace ECondo.Infrastructure.Data;

internal static class PropertyTypeSeedData
{
    public static readonly PropertyType OfficeType = new PropertyType
    {
        Id = Guid.Parse("69b47813-b8ac-4ed1-8e66-8e8b0cb52877"),
        Name = "Офис",
    };

    public static readonly PropertyType ApartmentType = new PropertyType
    {
        Id = Guid.Parse("eb5d6530-e91e-4509-8722-bc233d7b5354"),
        Name = "Апартамент",
    };

    public static readonly PropertyType StudioType = new PropertyType
    {
        Id = Guid.Parse("65822f30-fe07-478b-ae4a-0133d0af6be9"),
        Name = "Ателие",
    };

    public static readonly PropertyType GarageType = new PropertyType
    {
        Id = Guid.Parse("c5894037-547f-42c8-96d0-be536b957a0d"),
        Name = "Гараж",
    };

    public static readonly PropertyType ShopType = new PropertyType
    {
        Id = Guid.Parse("9d8afaac-8385-4162-a032-3c677db8970f"),
        Name = "Магазин",
    };

    public static readonly PropertyType BasementType = new PropertyType
    {
        Id = Guid.Parse("409bf101-d773-4557-a11c-3ea4e718a4bb"),
        Name = "Мазе",
    };

    public static readonly PropertyType WarehouseType = new PropertyType
    {
        Id = Guid.Parse("99d38747-cd6f-44b2-9e2c-971cd0ad119c"),
        Name = "Склад",
    };

    public static readonly PropertyType AtticType = new PropertyType
    {
        Id = Guid.Parse("bec76b18-66c4-46be-94c1-65c12bf524d2"),
        Name = "Таванско помещение",
    };

    public static readonly IEnumerable<PropertyType> PropertyTypes = [
        OfficeType,
        ApartmentType,
        StudioType,
        GarageType,
        ShopType,
        BasementType,
        WarehouseType,
        AtticType,
    ];
}