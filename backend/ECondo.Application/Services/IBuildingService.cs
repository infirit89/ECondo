using ECondo.Application.Data;

namespace ECondo.Application.Services;

public interface IBuildingService
{
    Task<BuildingResult[]> GetBuildingsForUser(Guid userId);
}