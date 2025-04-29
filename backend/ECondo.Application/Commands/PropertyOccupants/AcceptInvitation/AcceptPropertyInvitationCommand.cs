namespace ECondo.Application.Commands.PropertyOccupants.AcceptInvitation;

public sealed record AcceptPropertyInvitationCommand(Guid Token, string Email)
    : ICommand;
