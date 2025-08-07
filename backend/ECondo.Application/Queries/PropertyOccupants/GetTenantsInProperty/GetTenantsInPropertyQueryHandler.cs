using ECondo.Application.Data.Occupant;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Collections;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.PropertyOccupants.GetTenantsInProperty;

internal sealed class GetTenantsInPropertyQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetTenantsInPropertyQuery, PagedList<OccupantResult>>
{
    public async Task<Result<PagedList<OccupantResult>, Error>> Handle(GetTenantsInPropertyQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext
            .PropertyOccupants
            .Include(po => po.OccupantType)
            .AsNoTracking()
            .Where(po => 
                po.PropertyId == request.PropertyId &&
                po.OccupantType.Name != OccupantType.OwnerType)
            .Select(po =>
                new OccupantResult(
                    po.Id,
                    po.FirstName,
                    po.MiddleName,
                    po.LastName,
                    po.OccupantType.Name,
                    po.Email,
                    po.InvitationStatus))
            .ToPagedListAsync(request.Page, request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<OccupantResult>, Error>.Ok(result);
    }
}