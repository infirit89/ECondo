using ECondo.Domain.Shared;

namespace ECondo.Domain;

public static class PropertyTypeErrors
{
    public static Error InvalidPropertyType(string propertyType) =>
        Error.NotFound(
            "PropertyTypes.Invalid",
            $"Property type with name = '{propertyType}' is invalid");
}