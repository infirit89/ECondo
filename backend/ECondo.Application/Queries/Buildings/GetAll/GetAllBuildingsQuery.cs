using ECondo.Application.Data;
using ECondo.Application.Policies;
using ECondo.Application.Policies.Admin;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Buildings.GetAll;

public sealed record GetAllBuildingsQuery(int Page, int PageSize) : 
    IQuery<PagedList<BuildingResult>>, IIsAdmin;
