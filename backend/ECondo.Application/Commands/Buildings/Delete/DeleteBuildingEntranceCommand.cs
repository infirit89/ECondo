using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.Buildings.Delete;

public record DeleteBuildingEntranceCommand(
    Guid EntranceId) :
    ICommand, ICanUpdate<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}