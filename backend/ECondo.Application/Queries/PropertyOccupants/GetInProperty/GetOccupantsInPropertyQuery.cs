using ECondo.Application.Policies;

namespace ECondo.Application.Queries.PropertyOccupants.GetInProperty;

public sealed record OccupantResult(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Type,
    string? Email);

public sealed record GetOccupantsInPropertyQuery(Guid PropertyId)
    : IQuery<IEnumerable<OccupantResult>>, ICanEditProperty;
