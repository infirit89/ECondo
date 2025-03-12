namespace ECondo.Application.Data;

public sealed record TokenResult(string AccessToken, int ExpiresIn, string RefreshToken);
