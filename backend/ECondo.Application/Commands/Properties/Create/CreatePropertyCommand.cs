using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.Properties.Create;

public sealed record CreatePropertyCommand(
    Guid EntranceId,
    string PropertyType,
    string Floor,
    string Number,
    int BuiltArea,
    int IdealParts) : ICommand, ICanUpdate<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
