using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.Properties.Delete;

public sealed record DeletePropertyCommand(
    Guid PropertyId)
    : ICommand, ICanDelete<Property>
{
    Guid? IResourcePolicy.ResourceId => PropertyId;
}
