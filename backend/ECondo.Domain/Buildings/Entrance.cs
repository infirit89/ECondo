using ECondo.Domain.Users;

namespace ECondo.Domain.Buildings;

public sealed class Entrance
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BuildingId { get; set; }
    public Building Building { get; set; } = null!;

    public Guid ManagerId { get; set; }
    public User Manager { get; set; } = null!;
}