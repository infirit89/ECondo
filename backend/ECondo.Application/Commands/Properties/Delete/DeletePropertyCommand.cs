using ECondo.Application.Policies;
using ECondo.Application.Policies.Property;

namespace ECondo.Application.Commands.Properties.Delete;

public sealed record DeletePropertyCommand(
    Guid PropertyId)
    : ICommand, ICanEditProperty;
