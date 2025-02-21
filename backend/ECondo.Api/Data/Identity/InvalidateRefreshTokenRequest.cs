namespace ECondo.Api.Data.Identity;
public sealed record InvalidateRefreshTokenRequest(string Username, string RefreshToken);
