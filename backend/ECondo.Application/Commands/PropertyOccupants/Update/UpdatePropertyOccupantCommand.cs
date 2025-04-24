using ECondo.Application.Policies;

namespace ECondo.Application.Commands.PropertyOccupants.Update;

public sealed record UpdatePropertyOccupantCommand(
    Guid OccupantId,
    string FirstName,
    string MiddleName,
    string LastName,
    string Type,
    string? Email,
    string ReturnUri) : ICommand, ICanEditOccupant;
