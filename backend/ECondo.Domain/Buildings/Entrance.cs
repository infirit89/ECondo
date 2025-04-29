using ECondo.Domain.Payments;
using ECondo.Domain.Users;

namespace ECondo.Domain.Buildings;

public sealed class Entrance
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = null!;

    public Guid ManagerId { get; set; }
    public User Manager { get; set; } = null!;

    public string Number { get; set; } = null!;
    public string? StripeAccountId { get; set; }
    
    public HashSet<Property> Properties { get; set; } = [];

    public HashSet<Bill> Bills { get; set; } = [];
}