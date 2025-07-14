using ECondo.Application.Policies;
using ECondo.Application.Policies.Occupant;

namespace ECondo.Application.Commands.PropertyOccupants.Delete;

public sealed record DeletePropertyOccupantCommand(
    Guid OccupantId) : ICommand, ICanDeleteOccupant;
