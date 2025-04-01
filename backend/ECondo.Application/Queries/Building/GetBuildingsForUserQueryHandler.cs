using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Queries.Building;

internal sealed class GetBuildingsForUserQueryHandler(
    IUserContext userContext,
    IBuildingService buildingService)
    : IRequestHandler<
        GetBuildingsForUserQuery, Result<BuildingResult[], Error>>
{
    public async Task<Result<BuildingResult[], Error>> 
        Handle(GetBuildingsForUserQuery request, 
            CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<BuildingResult[], Error>
                .Fail(UserErrors.InvalidUser());

        var buildings = await buildingService
            .GetBuildingsForUser((Guid)userContext.UserId);
        return Result<BuildingResult[], Error>.Ok(buildings);
    }
}