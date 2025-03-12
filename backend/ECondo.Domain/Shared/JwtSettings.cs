namespace ECondo.Domain.Shared;

public class JwtSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
    public int MinutesExpiry { get; init; }
}
