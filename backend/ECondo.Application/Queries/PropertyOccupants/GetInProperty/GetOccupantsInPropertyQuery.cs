using ECondo.Application.Authorization.Policies.Property;
using ECondo.Application.Data.Occupant;

namespace ECondo.Application.Queries.PropertyOccupants.GetInProperty;

// TODO: make paged list
public sealed record GetOccupantsInPropertyQuery(Guid PropertyId)
    : IQuery<IEnumerable<OccupantResult>>, ICanEditProperty;
