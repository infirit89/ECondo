namespace ECondo.Application.Policies.Building;

public interface ICanEditEntrance
{
    public Guid BuildingId { get; init; }
    public string EntranceNumber { get; init; }
}
