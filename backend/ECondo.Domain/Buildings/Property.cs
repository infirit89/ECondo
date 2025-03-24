namespace ECondo.Domain.Buildings;

public sealed class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EntranceId { get; set; }
    public Entrance Entrance { get; set; } = null!;

    public int Floor { get; set; }
    public int Number { get; set; }

    public Guid PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;

    public int BuiltArea { get; set; }
    public int IdealParts { get; set; }

    // People that occupy the property but aren't users
    public HashSet<Occupant> Occupants { get; set; } = [];

    public HashSet<PropertyUser> PropertyUsers { get; set; } = [];
}
