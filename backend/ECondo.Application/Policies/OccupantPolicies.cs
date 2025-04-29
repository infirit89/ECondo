namespace ECondo.Application.Policies;

public interface ICanEditOccupant
{
    public Guid OccupantId { get; init; }
    public string Type { get; init; }
}

public interface ICanDeleteOccupant
{
    public Guid OccupantId { get; init; }
}

public interface ICanAddOccupant
{
    public Guid PropertyId { get; init; }
    public string OccupantType { get; init; }
}

public interface ICanSeeTenants
{
    public Guid PropertyId { get; init; }
}
