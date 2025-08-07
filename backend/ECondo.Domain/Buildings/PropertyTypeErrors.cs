using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Buildings;

public static class PropertyTypeErrors
{
    public static Error InvalidPropertyType(string propertyType) =>
        Error.NotFound(
            "PropertyTypes.Invalid",
            $"Property type with name = '{propertyType}' is invalid");
}