namespace ECondo.Application.Commands.Profiles.Update;

public sealed record UpdateProfileCommand(
    string FirstName, 
    string MiddleName, 
    string LastName) 
    : ICommand;
