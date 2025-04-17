using ECondo.Application.Data;

namespace ECondo.Application.Queries.Buildings.GetForUser;

public sealed record GetBuildingsForUserQuery
    : IQuery<BuildingResult[]>;
