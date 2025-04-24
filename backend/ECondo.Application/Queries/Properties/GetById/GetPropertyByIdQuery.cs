using ECondo.Application.Policies;

namespace ECondo.Application.Queries.Properties.GetById;

public sealed record PropertyResult(
    Guid Id,
    string Floor,
    string Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts);

public sealed record GetPropertyByIdQuery(
    Guid PropertyId) : IQuery<PropertyResult>, ICanSeeProperty;