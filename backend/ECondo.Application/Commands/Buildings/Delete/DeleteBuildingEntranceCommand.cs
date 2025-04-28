using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Buildings.Delete;

public record DeleteBuildingEntranceCommand(Guid BuildingId, string EntranceNumber) : 
    ICommand, ICanEditEntrance;