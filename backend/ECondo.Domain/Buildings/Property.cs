using ECondo.Domain.Payments;

namespace ECondo.Domain.Buildings;

public sealed class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EntranceId { get; set; }
    public Entrance Entrance { get; set; } = null!;

    public string Floor { get; set; } = null!;
    public string Number { get; set; } = null!;

    public Guid PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;

    public int BuiltArea { get; set; }
    public int IdealParts { get; set; }

    // People that occupy the property but may not be users
    public HashSet<PropertyOccupant> PropertyOccupants { get; set; } = [];
    public HashSet<Payment> Payments { get; set; } = [];
}
