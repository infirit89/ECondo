namespace ECondo.Domain.Shared;

public sealed class StripeSettings
{
    public string SecretKey { get; init; } = null!;
}