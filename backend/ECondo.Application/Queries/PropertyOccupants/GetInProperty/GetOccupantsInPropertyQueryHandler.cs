using ECondo.Application.Data.Occupant;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.PropertyOccupants.GetInProperty;

internal sealed class GetOccupantsInPropertyQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetOccupantsInPropertyQuery, IEnumerable<OccupantResult>>
{
    public async Task<Result<IEnumerable<OccupantResult>, Error>> Handle(
        GetOccupantsInPropertyQuery request, 
        CancellationToken cancellationToken)
    {
        var result = await dbContext
            .PropertyOccupants
            .Include(po => po.OccupantType)
            .AsNoTracking()
            .Where(po => po.PropertyId == request.PropertyId)
            .Select(po => new
            {
                po.Id,
                po.FirstName,
                po.MiddleName,
                po.LastName,
                Type = po.OccupantType.Name,
                po.Email,
                po.InvitationStatus,
            })
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<IEnumerable<OccupantResult>, Error>.Ok(result.Select(po =>
            new OccupantResult(
                po.Id, 
                po.FirstName, 
                po.MiddleName, 
                po.LastName, 
                po.Type, 
                po.Email,
                po.InvitationStatus)));
    }
}