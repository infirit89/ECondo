using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class EntranceErrors
{
    public static Error AlreadyExists(Guid buildingId, string entranceNumber) =>
    new()
    {
        Code = nameof(AlreadyExists),
        Description = $"The entrance {entranceNumber} for building {buildingId} already exists",
    };
}
