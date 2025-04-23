using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class PropertyOccupantError
{
    public static Error Duplicate(string email, string occupantType) =>
        Error.Conflict(
            "PropertyOccupants.Duplicate",
            $"An occupant with email '{email}' and type '{occupantType}' already exists for this property.");
}