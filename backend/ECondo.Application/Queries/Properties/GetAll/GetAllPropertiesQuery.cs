using ECondo.Application.Data.Property;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Properties.GetAll;

public sealed record EntranceFilter(Guid BuildingId, string EntranceNumber);

public sealed record GetAllPropertiesQuery(
    int Page,
    int PageSize,
    EntranceFilter? EntranceFilter)
    : IQuery<PagedList<PropertyOccupantResult>>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => null;
}