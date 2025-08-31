using ECondo.Application.Data;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Buildings.GetAll;

public sealed record GetAllBuildingsQuery(int Page, int PageSize) :
    IQuery<PagedList<BuildingResult>>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => null;
}
