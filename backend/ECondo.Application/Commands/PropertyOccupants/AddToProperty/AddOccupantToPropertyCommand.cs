using ECondo.Application.Policies;

namespace ECondo.Application.Commands.PropertyOccupants.AddToProperty;

public sealed record AddOccupantToPropertyCommand(
    Guid PropertyId,
    string FirstName,
    string MiddleName,
    string LastName,
    string OccupantType,
    string? Email,
    string ReturnUri)
    : ICommand, ICanAddOccupant;
