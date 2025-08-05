using ECondo.Application.Data.Property;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Queries.Properties.GetById;

public sealed record GetPropertyByIdQuery(
    Guid PropertyId) : IQuery<PropertyResult>, ICanRead<Property>
{
    Guid? IResourcePolicy.ResourceId => PropertyId;
}