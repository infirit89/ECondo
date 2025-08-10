namespace ECondo.Domain.Buildings;

public sealed class OccupantType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = null!;

    public HashSet<PropertyOccupant> PropertyOccupants { get; set; } = [];

    public const string TenantType = "tennat";
    public const string OwnerType = "owner";
    public const string UserType = "user";
    public const string RepresentativeType = "representative";
}
