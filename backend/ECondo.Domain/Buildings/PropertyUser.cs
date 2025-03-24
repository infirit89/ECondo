using ECondo.Domain.Users;

namespace ECondo.Domain.Buildings;

public sealed class PropertyUser
{
    public Guid PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid OccupantTypeId { get; set; }
    public OccupantType OccupantType { get; set; } = null!;
}
