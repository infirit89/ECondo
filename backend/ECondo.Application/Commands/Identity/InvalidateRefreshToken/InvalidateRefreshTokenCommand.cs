namespace ECondo.Application.Commands.Identity.InvalidateRefreshToken;

public sealed record InvalidateRefreshTokenCommand(
    string RefreshToken) : ICommand;
