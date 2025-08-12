using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.PropertyOccupants.Update;

public sealed record UpdatePropertyOccupantCommand(
    Guid OccupantId,
    string FirstName,
    string MiddleName,
    string LastName,
    string Type,
    string? Email,
    string ReturnUri) : ICommand, ICanUpdate<PropertyOccupant>
{
    Guid? IResourcePolicy.ResourceId => OccupantId;
}
