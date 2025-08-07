using ECondo.Application.Data.Occupant;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.PropertyOccupants.GetTenantsInProperty;

public record GetTenantsInPropertyQuery(Guid PropertyId, int Page, int PageSize) 
    : IQuery<PagedList<OccupantResult>>;