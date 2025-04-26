using ECondo.Application.Data;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Buildings.GetForUser;

public sealed record GetBuildingsForUserQuery(int Page, int PageSize)
    : IQuery<PagedList<BuildingResult>>;
