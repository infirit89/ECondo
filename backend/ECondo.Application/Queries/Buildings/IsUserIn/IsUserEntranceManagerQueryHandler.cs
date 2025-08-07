using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Buildings.IsUserIn;

internal sealed class IsUserEntranceManagerQueryHandler(
    IUserContext userContext,
    IApplicationDbContext dbContext)
    : IQueryHandler<IsUserEntranceManagerQuery>
{
    public async Task<Result<EmptySuccess, Error>> 
        Handle(
            IsUserEntranceManagerQuery request,
            CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .AsNoTracking()
            .AnyAsync(e =>
            e.ManagerId == userContext.UserId &&
            e.BuildingId == request.BuildingId &&
            e.Number == request.EntranceNumber,
            cancellationToken: cancellationToken);

        if(!entrance)
            return Result<EmptySuccess, Error>
                .Fail(BuildingErrors.InvalidAccess());

        return Result<EmptySuccess, Error>.Ok();
    }
}
