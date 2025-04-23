using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class PropertyErrors
{
    public static Error AlreadyExists(int number, Guid entranceId) =>
        Error.Conflict(
            "Properties.AlreadyExists",
            $"The property with number = {number} already exists in entrance '{entranceId}'");

    public static Error InvalidProperty(Guid propertyId) =>
        Error.NotFound(
            "Properties.NotFound",
            $"The property with Id = '{propertyId}' does not exists");
}