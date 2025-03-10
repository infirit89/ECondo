using ECondo.Application.Services;
using Resend;

namespace ECondo.Infrastructure.Services;

internal class MailService(IResend resend) : IEmailService
{
    public async Task SendAccountConfirmationMail(string recipientMail, string confirmationLink)
    {
        var message = new EmailMessage
        {
            From = "onboarding@resend.dev",
            Subject = "Account Confirmation",
            HtmlBody = $"<a href=\"{confirmationLink}\">Confirm Account</a>"
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }

    public async Task SendPasswordResetMail(string recipientMail, string resetLink)
    {
        var message = new EmailMessage
        {
            From = "onboarding@resend.dev",
            Subject = "Reset NewPassword",
            HtmlBody = $"<a href=\"{resetLink}\">Reset NewPassword</a>"
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }
}
