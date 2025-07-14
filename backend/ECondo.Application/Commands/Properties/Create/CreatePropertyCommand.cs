using ECondo.Application.Policies;
using ECondo.Application.Policies.EntranceManager;
using ECondo.Application.Policies.Property;

namespace ECondo.Application.Commands.Properties.Create;

public sealed record CreatePropertyCommand(
    Guid BuildingId,
    string EntranceNumber,
    string PropertyType,
    string Floor,
    string Number,
    int BuiltArea,
    int IdealParts) : ICommand, ICanAddProperty;
