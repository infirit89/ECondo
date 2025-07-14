using ECondo.Application.Policies;
using ECondo.Application.Policies.Building;

namespace ECondo.Application.Commands.Buildings.Delete;

public record DeleteBuildingEntranceCommand(Guid BuildingId, string EntranceNumber) : 
    ICommand, ICanEditEntrance;