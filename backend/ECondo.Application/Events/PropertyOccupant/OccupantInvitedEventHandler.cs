using ECondo.Application.Services;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace ECondo.Application.Events.PropertyOccupant;

internal sealed class OccupantInvitedEventHandler(
    IEmailService emailService)
    : INotificationHandler<OccupantInvitedEvent>
{
    public async Task Handle(OccupantInvitedEvent notification, CancellationToken cancellationToken)
    {
        Dictionary<string, string?> tokenParams = new Dictionary<string, string?>
        {
            { "token", notification.Token.ToString()! },
            { "email", notification.Email },
        };

        string returnUrl = QueryHelpers.AddQueryString(notification.ReturnUri, tokenParams);

        await emailService.SendInvitationEmail(
            notification.Email, 
            returnUrl, 
            notification.FirstName,
            notification.InvitationExpiry);
    }
}