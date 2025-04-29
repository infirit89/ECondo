namespace ECondo.Application.Queries.PropertyOccupants.IsUser;

public record UserOccupantResult(string OccupantType);

public record IsUserPropertyOccupantQuery(Guid PropertyId) : IQuery<UserOccupantResult>;