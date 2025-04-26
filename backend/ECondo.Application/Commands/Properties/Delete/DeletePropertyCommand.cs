using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Properties.Delete;

public sealed record DeletePropertyCommand(
    Guid PropertyId)
    : ICommand, ICanEditProperty;
