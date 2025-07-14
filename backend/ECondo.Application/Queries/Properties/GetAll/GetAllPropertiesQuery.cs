using ECondo.Application.Data;
using ECondo.Application.Data.Property;
using ECondo.Application.Policies;
using ECondo.Application.Policies.Admin;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Properties.GetAll;

public sealed record EntranceFilter(Guid BuildingId, string EntranceNumber);
public sealed record GetAllPropertiesQuery(
    int Page, 
    int PageSize, 
    EntranceFilter? EntranceFilter) 
    : IQuery<PagedList<PropertyOccupantResult>>, IIsAdmin;