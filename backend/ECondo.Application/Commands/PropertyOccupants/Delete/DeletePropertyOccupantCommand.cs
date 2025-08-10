using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.PropertyOccupants.Delete;

public sealed record DeletePropertyOccupantCommand(
    Guid OccupantId) : ICommand, ICanDelete<PropertyOccupant>
{
    Guid? IResourcePolicy.ResourceId => OccupantId;
}
