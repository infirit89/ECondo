namespace ECondo.Application.Commands.Identity.ResetPassword;

public sealed record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword)
    : ICommand;
