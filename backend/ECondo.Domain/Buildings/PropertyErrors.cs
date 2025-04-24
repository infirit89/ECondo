using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class PropertyErrors
{
    public static Error AlreadyExists(string number, Guid entranceId) =>
        Error.Conflict(
            "Properties.AlreadyExists",
            $"The property with number = {number} already exists in entrance '{entranceId}'");

    public static Error InvalidProperty(Guid propertyId) =>
        Error.NotFound(
            "Properties.NotFound",
            $"The property with Id = '{propertyId}' does not exists");

    public static Error Forbidden(Guid propertyId) =>
        Error.Forbidden(
            "Properties.Forbidden",
            $"The user can not access the property with id '{propertyId}'");
}