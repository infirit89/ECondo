namespace ECondo.Domain.Shared;

public sealed class RefreshTokenSettings
{
    public int DaysExpiry { get; init; }
    public int Length { get; init; }
}
