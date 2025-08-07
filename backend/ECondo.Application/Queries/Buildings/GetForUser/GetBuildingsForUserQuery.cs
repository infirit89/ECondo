using ECondo.Application.Data;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Buildings.GetForUser;

public sealed record GetBuildingsForUserQuery(
    int Page, int PageSize, string? BuildingName)
    : IQuery<PagedList<BuildingResult>>;
