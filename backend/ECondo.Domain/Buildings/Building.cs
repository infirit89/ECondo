namespace ECondo.Domain.Buildings;

public sealed class Building
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public Guid ProvinceId { get; set; }
    public Province Province { get; set; } = null!;

    public string Municipality { get; set; } = null!;
    public string SettlementPlace { get; set; } = null!;

    public string Neighborhood { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string StreetNumber { get; set; } = null!;
    public string BuildingNumber { get; set; } = null!;

    public HashSet<Entrance> Entrances { get; set; } = new();
}