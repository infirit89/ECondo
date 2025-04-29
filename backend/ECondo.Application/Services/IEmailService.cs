namespace ECondo.Application.Services;

public interface IEmailService
{
    Task SendAccountConfirmationMail(string recipientMail, string confirmationLink);
    Task SendPasswordResetMail(string recipientMail, string resetLink);
    Task SendInvitationEmail(
        string recipientMail,
        string invitationLink,
        string firstName,
        DateTimeOffset expiresAt);
}
