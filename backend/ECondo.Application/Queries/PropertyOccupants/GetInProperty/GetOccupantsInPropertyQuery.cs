using ECondo.Application.Data.Occupant;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Queries.PropertyOccupants.GetInProperty;

// TODO: make paged list
public sealed record GetOccupantsInPropertyQuery(Guid PropertyId)
    : IQuery<IEnumerable<OccupantResult>>, ICanRead<Property>
{
    Guid? IResourcePolicy.ResourceId => PropertyId;
}
