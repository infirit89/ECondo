using ECondo.Application.Authorization.Policies.Property;
using ECondo.Application.Data.Property;

namespace ECondo.Application.Queries.Properties.GetById;

public sealed record GetPropertyByIdQuery(
    Guid PropertyId) : IQuery<PropertyResult>, ICanSeeProperty;