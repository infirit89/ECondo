namespace ECondo.Domain.Shared;

public sealed class RefreshToken
{
    public required string Value { get; init; }
    public DateTime Expires { get; init; }
    public Guid UserId { get; set; }
}
