using ECondo.Application.Data.Property;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Properties.GetInEntrance;

public sealed record GetPropertiesInEntranceQuery(
    Guid EntranceId,
    int Page,
    int PageSize)
    : IQuery<PagedList<PropertyOccupantResult>>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
