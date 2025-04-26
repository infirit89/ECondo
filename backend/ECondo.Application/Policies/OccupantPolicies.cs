namespace ECondo.Application.Policies;

public interface ICanEditOccupant
{
    public Guid OccupantId { get; init; }
}

public interface ICanAddOccupant
{
    public Guid PropertyId { get; init; }
}