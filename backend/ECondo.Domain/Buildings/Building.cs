namespace ECondo.Domain.Buildings;

public sealed class Building
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public Guid CityId { get; set; }
    public City City { get; set; } = null!;

    public string Neighborhood { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string StreetNumber { get; set; } = null!;
    public string BuildingNumber { get; set; } = null!;

    public HashSet<Entrance> Entrances { get; set; } = new();
}