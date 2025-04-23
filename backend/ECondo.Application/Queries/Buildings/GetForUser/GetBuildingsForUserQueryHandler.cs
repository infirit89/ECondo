using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Buildings.GetForUser;

internal sealed class GetBuildingsForUserQueryHandler(
    IUserContext userContext,
    IBuildingService buildingService)
    : IQueryHandler<
        GetBuildingsForUserQuery, BuildingResult[]>
{
    public async Task<Result<BuildingResult[], Error>> 
        Handle(GetBuildingsForUserQuery request, 
            CancellationToken cancellationToken)
    {
        var buildings = await buildingService
            .GetBuildingsForUser(userContext.UserId);
        return Result<BuildingResult[], Error>.Ok(buildings);
    }
}