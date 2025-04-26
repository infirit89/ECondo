using System.Reflection;
using ECondo.Application.Services;
using Resend;

namespace ECondo.Infrastructure.Services;

// NOTE: the email HTML SHOULD NOT BE HERE, im leaving it for now cause too much work
// TODO: SHOULD BE IN ANOTHER FILE, MAYBE EVEN ANOTHER PROJECT!!!
internal class MailService(IResend resend) : IEmailService
{
    public async Task SendAccountConfirmationMail(string recipientMail, string confirmationLink)
    {
        var templateDirectory = Path.Combine(
            Directory.GetCurrentDirectory(), "EmailTemplates");

        var templatePath = Path.Combine(templateDirectory, "RegisterTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);
        
        // todo: this html should be in a separate file
        var message = new EmailMessage
        {
            From = "support@econdo.online",
            Subject = "Account Confirmation",
            HtmlBody = ""
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }

    public async Task SendPasswordResetMail(string recipientMail, string resetLink)
    {
        // todo: this html should be in a separate file
        var message = new EmailMessage
        {
            From = "support@econdo.online",
            Subject = "Reset NewPassword",
            HtmlBody = ""
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }

    public Task SendInvitationEmail(string recipientMail, string invitationLink, string firstName, DateTimeOffset expiresAt)
    {

        // todo: this html should be in a separate file
        var message = new EmailMessage
        {
            From = "invitations@econdo.online",
            Subject = "You've been invited to join ECondo",
            HtmlBody = ""
        };

        message.To.Add(recipientMail);
        return resend.EmailSendAsync(message);
    }
}
