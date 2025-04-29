namespace ECondo.Application.Commands.Identity.Register;

public sealed record RegisterCommand(
    string Email,
    string Username,
    string Password,
    string ReturnUri) 
    : ICommand;
