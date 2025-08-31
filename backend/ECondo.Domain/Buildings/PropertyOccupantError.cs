using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Buildings;

public static class PropertyOccupantError
{
    public static Error Duplicate(string email, string occupantType) =>
        Error.Conflict(
            "PropertyOccupants.Duplicate",
            $"An occupant with email '{email}' and type '{occupantType}' already exists for this property.");

    public static Error NotFound(string email) =>
        Error.NotFound(
            "PropertyOccupants.NotFound",
            $"The occupant with email '{email}' was not found.");

    public static Error InvitationExpired() =>
        Error.Forbidden(
            "PropertyOccupants.InvalidInvitation",
            $"The invitation for the property is invalid or expired");

    public static Error Forbidden(Guid occupantId) =>
        Error.Forbidden(
            "PropertyOccupants.Forbidden",
            $"The user ca not access the occupant with id '{occupantId}'");
}