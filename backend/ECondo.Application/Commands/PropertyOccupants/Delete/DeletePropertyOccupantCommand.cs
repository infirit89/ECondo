using ECondo.Application.Policies;

namespace ECondo.Application.Commands.PropertyOccupants.Delete;

public sealed record DeletePropertyOccupantCommand(
    Guid OccupantId) : ICommand, ICanEditOccupant;
