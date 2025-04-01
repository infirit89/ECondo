using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class BuildingErrors
{
    public static Error InvalidBuilding(Guid id) => new()
    {
        Code = nameof(InvalidBuilding),
        Description = $"The building with Id = '{id}' was not found",
    };
}
