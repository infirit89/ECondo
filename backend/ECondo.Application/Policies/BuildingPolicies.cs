namespace ECondo.Application.Policies;

public interface ICanEditEntrance
{
    public Guid BuildingId { get; init; }
    public string EntranceNumber { get; init; }
}
