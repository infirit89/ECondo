namespace ECondo.Application.Policies;

public interface ICanEditOccupant
{
    public Guid OccupantId { get; init; }
}