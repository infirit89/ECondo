namespace ECondo.Domain.Buildings;

public sealed class OccupantType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public HashSet<Occupant> Occupants { get; set; } = [];
    public HashSet<PropertyUser> PropertyUsers { get; set; } = [];
}
