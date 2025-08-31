using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.Properties.Update;

public sealed record UpdatePropertyCommand(
    Guid PropertyId,
    string Floor,
    string Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts) : ICommand, ICanUpdate<Property>
{
    Guid? IResourcePolicy.ResourceId => PropertyId;
}