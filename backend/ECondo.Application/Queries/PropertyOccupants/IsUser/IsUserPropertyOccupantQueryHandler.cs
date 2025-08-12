using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.PropertyOccupants.IsUser;

public class IsUserPropertyOccupantQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<IsUserPropertyOccupantQuery, UserOccupantResult>
{
    public async Task<Result<UserOccupantResult, Error>> Handle(
        IsUserPropertyOccupantQuery request, 
        CancellationToken cancellationToken)
    {
        var occupant = await dbContext
            .PropertyOccupants
            .Include(po => po.OccupantType)
            .AsNoTracking()
            .Where(po => po.PropertyId == request.PropertyId && 
                         po.UserId == userContext.UserId)
            .Select(po => new { Type = po.OccupantType.Name })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if(occupant is null)
            return Result<UserOccupantResult, Error>.Fail(PropertyErrors.Forbidden(request.PropertyId));
        
        return Result<UserOccupantResult, Error>.Ok(new UserOccupantResult(occupant.Type));
    }
}