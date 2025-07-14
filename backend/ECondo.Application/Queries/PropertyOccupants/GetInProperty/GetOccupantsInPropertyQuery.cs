using ECondo.Application.Data.Occupant;
using ECondo.Application.Policies;
using ECondo.Application.Policies.Property;

namespace ECondo.Application.Queries.PropertyOccupants.GetInProperty;

// TODO: make paged list
public sealed record GetOccupantsInPropertyQuery(Guid PropertyId)
    : IQuery<IEnumerable<OccupantResult>>, ICanEditProperty;
