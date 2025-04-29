namespace ECondo.Application.Commands.Identity.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Username,
    string ReturnUri)
    : ICommand;
