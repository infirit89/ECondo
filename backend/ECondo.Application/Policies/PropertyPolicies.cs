namespace ECondo.Application.Policies;

public interface ICanSeeProperty
{
    public Guid PropertyId { get; init; }
}

public interface ICanEditProperty
{
    public Guid PropertyId { get; init; }
}

// public interface ICanCreateProperty
// {
//     public Guid BuildingId { get; init; }
//     public string EntranceNumber { get; init; }
// }
