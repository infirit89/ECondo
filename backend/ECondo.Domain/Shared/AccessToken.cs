namespace ECondo.Domain.Shared;

public class AccessToken
{
    public required string Value { get; init; }
    public int MinutesExpiry { get; init; }
}
