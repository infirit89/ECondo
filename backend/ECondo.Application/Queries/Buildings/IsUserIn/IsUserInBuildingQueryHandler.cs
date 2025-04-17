using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Buildings.IsUserIn;

internal sealed class IsUserInBuildingQueryHandler(
    IUserContext userContext,
    IApplicationDbContext dbContext)
    : IQueryHandler<IsUserInBuildingQuery>
{
    public async Task<Result<EmptySuccess, Error>> 
        Handle(
            IsUserInBuildingQuery request,
            CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser());

        var entrance = await dbContext
            .Entrances
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
            e.ManagerId == userContext.UserId &&
            e.BuildingId == request.BuildingId,
            cancellationToken: cancellationToken);

        if(entrance is null)
            return Result<EmptySuccess, Error>
                .Fail(BuildingErrors.InvalidAccess());

        return Result<EmptySuccess, Error>.Ok();
    }
}
