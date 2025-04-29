using ECondo.Application.Data;

namespace ECondo.Application.Commands.Identity.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<TokenResult>;
