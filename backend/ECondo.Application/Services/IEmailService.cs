namespace ECondo.Application.Services;

public interface IEmailService
{
    Task SendAccountConfirmationMail(string recipientMail, string confirmationLink);
    Task SendPasswordResetMail(string recipientMail, string resetLink);
}
