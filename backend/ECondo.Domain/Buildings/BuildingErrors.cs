using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class BuildingErrors
{
    public static Error InvalidBuilding(Guid id) =>
        Error.NotFound("Building.NotFound", $"The building with Id = '{id}' was not found");

    public static Error InvalidAccess() =>
        Error.Conflict("Building.InvalidAccess", "The user does not have access to the building");
}
