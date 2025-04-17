namespace ECondo.Application.Commands.Identity.UpdatePassword;

public sealed record UpdatePasswordCommand(
    string CurrentPassword, 
    string NewPassword) 
    : ICommand;
