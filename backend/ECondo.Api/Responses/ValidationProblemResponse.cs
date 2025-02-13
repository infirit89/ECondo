namespace ECondo.Api.Responses;
public sealed class ValidationProblemResponse
{
    public Dictionary<string, string[]> Data { get; init; } = null!;
}
