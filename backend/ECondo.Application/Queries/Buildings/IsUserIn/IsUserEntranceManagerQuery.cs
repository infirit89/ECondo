namespace ECondo.Application.Queries.Buildings.IsUserIn;

public sealed record IsUserEntranceManagerQuery(
    Guid BuildingId, string EntranceNumber) : IQuery;
