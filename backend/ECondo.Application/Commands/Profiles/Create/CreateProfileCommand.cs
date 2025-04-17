namespace ECondo.Application.Commands.Profiles.Create;

public sealed record CreateProfileCommand(
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string PhoneNumber) 
    : ICommand;
