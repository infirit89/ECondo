using ECondo.Domain.Buildings;

namespace ECondo.Domain.Provinces;

public sealed class Province
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public HashSet<Building> Buildings { get; set; } = [];
}