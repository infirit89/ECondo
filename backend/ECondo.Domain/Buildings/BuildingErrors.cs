using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class BuildingErrors
{
    public static Error InvalidBuilding(Guid id) => new()
    {
        Code = nameof(InvalidBuilding),
        Description = $"The building with Id = '{id}' was not found",
    };

    public static Error InvalidAccess() => new()
    {
        Code = nameof(InvalidAccess),
        Description = "The user does not have access to the building",
    };
}
