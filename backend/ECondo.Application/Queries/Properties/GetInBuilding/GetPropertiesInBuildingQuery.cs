using ECondo.Application.Data;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Properties.GetInBuilding;

public sealed record GetPropertiesInBuildingQuery(
    Guid BuildingId,
    string EntranceNumber,
    int Page,
    int PageSize)
    : IQuery<PagedList<PropertyOccupantResult>>;
