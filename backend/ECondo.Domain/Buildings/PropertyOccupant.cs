using ECondo.Domain.Users;

namespace ECondo.Domain.Buildings;

public sealed class PropertyOccupant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PropertyId { get; set; }
    public Property Property { get; set; } = null!;


    public Guid OccupantTypeId { get; set; }
    public OccupantType OccupantType { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? Email { get; set; }
    public Guid? InvitationToken { get; set; }
    public DateTimeOffset? InvitationSentAt { get; set; }
    public DateTimeOffset? InvitationExpiresAt { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public bool IsInvitationExpired() =>
        InvitationToken is not null &&
        InvitationExpiresAt.HasValue &&
        InvitationExpiresAt.Value < DateTimeOffset.UtcNow;
}
