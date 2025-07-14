using ECondo.Application.Data;
using ECondo.Application.Data.Property;
using ECondo.Application.Policies;
using ECondo.Application.Policies.Property;

namespace ECondo.Application.Queries.Properties.GetById;

public sealed record GetPropertyByIdQuery(
    Guid PropertyId) : IQuery<PropertyResult>, ICanSeeProperty;