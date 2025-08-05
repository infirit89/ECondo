using ECondo.Domain.Shared;

namespace ECondo.Domain.Buildings;

public static class EntranceErrors
{
    public static Error AlreadyExists(Guid buildingId, string entranceNumber) =>
        Error.Conflict("Entrance.AlreadyExists",
            $"The entrance {entranceNumber} for building {buildingId} already exists");

    public static Error InvalidEntrance(Guid entranceId) =>
        Error.NotFound("Entrance.Invalid",
            $"The entrance with id = {entranceId} is invalid");
    
    public static Error Forbidden(Guid buildingId, string entranceNumber) =>
        Error.Forbidden("Entrance.Forbidden",
            $"The user doesn't have access to entrance {entranceNumber} for building {buildingId}");
}
