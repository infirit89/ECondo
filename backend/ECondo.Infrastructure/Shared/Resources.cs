namespace ECondo.Infrastructure.Shared;

internal static class Resources
{
    public const string DbConnectionError = "Failed to find connection string \"ECondo\"";
    public const string RedisConnectionError = "Failed to find the connection string for Redis";

    public const int LongName = 255;
    public const int ShortName = 128;
}
