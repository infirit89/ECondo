using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.PropertyOccupants.AddToProperty;

public sealed record AddOccupantToPropertyCommand(
    Guid PropertyId,
    string FirstName,
    string MiddleName,
    string LastName,
    string OccupantType,
    string? Email,
    string ReturnUri)
    : ICommand, ICanCreate<Property>
{
    Guid? IResourcePolicy.ResourceId => PropertyId;
}
