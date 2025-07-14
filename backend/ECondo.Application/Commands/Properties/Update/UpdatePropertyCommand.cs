using ECondo.Application.Policies;
using ECondo.Application.Policies.Property;

namespace ECondo.Application.Commands.Properties.Update;

public sealed record UpdatePropertyCommand(
    Guid PropertyId,
    string Floor,
    string Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts) : ICommand, ICanEditProperty;