namespace ECondo.Application.Commands.Identity.ConfirmEmail;

public sealed record ConfirmEmailCommand(
    string Token,
    string Email)
    : ICommand;
