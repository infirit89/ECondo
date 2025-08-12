using ECondo.Application.Data.Property;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Properties.GetForUser;

public record GetPropertiesForUserQuery(int Page, int PageSize) 
    : IQuery<PagedList<PropertyOccupantResult>>;