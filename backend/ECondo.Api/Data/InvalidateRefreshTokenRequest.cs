namespace ECondo.Api.Data;
public sealed record InvalidateRefreshTokenRequest(string Username, string RefreshToken);
