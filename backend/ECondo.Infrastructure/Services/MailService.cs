using ECondo.Application.Services;
using Resend;

namespace ECondo.Infrastructure.Services;

internal class MailService(IResend resend, IEmailTemplateService templateService) : IEmailService
{
    public async Task SendAccountConfirmationMail(string recipientMail, string confirmationLink)
    {
        string emailBody = await templateService.RenderTemplateAsync(
            "RegisterTemplate.html", new { confirmationLink });
        
        var message = new EmailMessage
        {
            From = "noreply@econdo.online",
            Subject = "Account Confirmation",
            HtmlBody = emailBody,
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }

    public async Task SendPasswordResetMail(string recipientMail, string resetLink)
    {
        string emailBody = await templateService.RenderTemplateAsync(
            "PasswordResetTemplate.html", new { resetLink });

        
        var message = new EmailMessage
        {
            From = "noreply@econdo.online",
            Subject = "Reset NewPassword",
            HtmlBody = emailBody,
        };
        message.To.Add(recipientMail);

        await resend.EmailSendAsync(message);
    }

    public async Task SendInvitationEmail(string recipientMail, string invitationLink, string firstName, DateTimeOffset expiresAt)
    {
        string emailBody = await templateService.RenderTemplateAsync(
            "PropertyInvitationTemplate.html", 
            new { firstName, invitationLink, expiresAt = $"{expiresAt:f}" });
        
        var message = new EmailMessage
        {
            From = "noreply@econdo.online",
            Subject = "You've been invited to join ECondo",
            HtmlBody = emailBody,
        };

        message.To.Add(recipientMail);
        await resend.EmailSendAsync(message);
    }
}
