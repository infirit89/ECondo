using ECondo.Application.Data.Property;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Properties.GetInBuilding;

public sealed record GetPropertiesInBuildingQuery(
    Guid BuildingId,
    string EntranceNumber,
    int Page,
    int PageSize)
    : IQuery<PagedList<PropertyOccupantResult>>;
