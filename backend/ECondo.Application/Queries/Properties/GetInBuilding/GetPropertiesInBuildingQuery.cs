using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Properties.GetInBuilding;

public sealed record BriefPropertyResult(
    Guid Id,
    string Floor,
    string Number,
    string PropertyType);

public sealed record GetPropertiesInBuildingQuery(
    Guid BuildingId,
    string EntranceNumber,
    int Page,
    int PageSize)
    : IQuery<PagedList<BriefPropertyResult>>;
