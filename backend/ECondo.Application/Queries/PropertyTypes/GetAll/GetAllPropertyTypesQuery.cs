using ECondo.Application.Commands;

namespace ECondo.Application.Queries.PropertyTypes.GetAll;

public sealed record PropertyTypeNameResult(
    IEnumerable<string> PropertyTypes);

public sealed record GetAllPropertyTypesQuery
    : ICommand<PropertyTypeNameResult>;
