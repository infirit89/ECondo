namespace ECondo.Domain.Buildings;

public sealed class Occupant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public Guid OccupantTypeId { get; set; }
    public OccupantType OccupantType { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? AccessCode { get; set; }
}
