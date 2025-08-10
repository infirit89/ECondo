using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Buildings;

public static class OccupantTypeErrors
{
    public static Error Invalid(string occupantType) =>
        Error.NotFound(
            "OccupantTypes.NotFound",
            $"Occupant type with name = '{occupantType}' was not found");
}