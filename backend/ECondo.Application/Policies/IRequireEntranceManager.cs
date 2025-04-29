namespace ECondo.Application.Policies;

internal interface IRequireEntranceManager
{
    Guid BuildingId { get; init; }
    string EntranceNumber { get; init; }
}
