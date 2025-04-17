namespace ECondo.Application.Queries.Buildings.IsUserIn;

public sealed record IsUserInBuildingQuery(Guid BuildingId) 
    : IQuery;
