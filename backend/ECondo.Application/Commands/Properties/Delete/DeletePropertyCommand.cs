using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Properties.Delete;

public sealed record DeletePropertyCommand(
    Guid BuildingId,
    string EntranceNumber,
    Guid PropertyId)
    : ICommand, IRequireEntranceManager;
