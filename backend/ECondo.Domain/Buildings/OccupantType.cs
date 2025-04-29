namespace ECondo.Domain.Buildings;

public sealed class OccupantType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public HashSet<PropertyOccupant> PropertyOccupants { get; set; } = [];
}
