using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Properties.Update;

public sealed record UpdatePropertyCommand(
    Guid BuildingId,
    string EntranceNumber,
    Guid PropertyId,
    int Floor,
    int Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts) : ICommand, IRequireEntranceManager;