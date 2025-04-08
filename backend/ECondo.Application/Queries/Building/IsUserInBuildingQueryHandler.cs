using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Queries.Building;

internal sealed class IsUserInBuildingQueryHandler(
    IUserContext userContext,
    IUnitOfWork unitOfWork)
    : IRequestHandler<IsUserInBuildingQuery, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>> Handle(IsUserInBuildingQuery request, CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>.Fail(UserErrors.InvalidUser());

        var entrance = await unitOfWork.Entrances.FirstOrDefaultAsync(e =>
            e.ManagerId == userContext.UserId && e.BuildingId == request.BuildingId);

        if(entrance is null)
            return Result<EmptySuccess, Error>.Fail(BuildingErrors.InvalidAccess());

        return Result<EmptySuccess, Error>.Ok();
    }
}
