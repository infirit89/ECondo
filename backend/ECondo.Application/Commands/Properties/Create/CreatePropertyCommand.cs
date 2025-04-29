using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Properties.Create;

public sealed record CreatePropertyCommand(
    Guid BuildingId,
    string EntranceNumber,
    string PropertyType,
    string Floor,
    string Number,
    int BuiltArea,
    int IdealParts) : ICommand, IRequireEntranceManager;
