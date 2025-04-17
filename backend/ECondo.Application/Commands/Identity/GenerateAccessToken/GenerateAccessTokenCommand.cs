using ECondo.Application.Data;

namespace ECondo.Application.Commands.Identity.GenerateAccessToken;

public sealed record GenerateAccessTokenCommand(string RefreshToken) 
    : ICommand<TokenResult>;
