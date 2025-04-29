namespace ECondo.Application.Queries.OccupantTypes.GetAll;

public sealed record OccupantTypeNameResult(
    IEnumerable<string> OccupantTypes);


public sealed record GetAllOccupantTypesQuery 
    : IQuery<OccupantTypeNameResult>;
