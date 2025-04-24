using MediatR;

namespace ECondo.Application.Events.PropertyOccupant;

public sealed record OccupantInvitedEvent(
    Guid? Token,
    string Email, 
    string FirstName, 
    DateTimeOffset InvitationExpiry,
    string ReturnUri) : INotification;
    