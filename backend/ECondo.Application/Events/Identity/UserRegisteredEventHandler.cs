using ECondo.Application.Services;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace ECondo.Application.Events.Identity;
internal sealed class UserRegisteredEventHandler(
    IEmailService emailService,
    UserManager<User> userManager) : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        string emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(notification.User);

        Dictionary<string, string?> tokenParams = new Dictionary<string, string?>()
        {
            { "token", emailConfirmationToken },
            { "email", notification.User.Email },
        };

        string returnUrl = QueryHelpers.AddQueryString(notification.ReturnUri, tokenParams);

        await emailService.SendAccountConfirmationMail(notification.User.Email!, returnUrl);
    }
}
