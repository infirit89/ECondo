namespace ECondo.Domain.Buildings;

public sealed class PropertyType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public HashSet<Property> Properties { get; set; } = [];
}