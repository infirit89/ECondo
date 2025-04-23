using ECondo.Application.Services;
using Resend;

namespace ECondo.Infrastructure.Services;

// NOTE: the email HTML SHOULD NOT BE HERE, im leaving it for now cause too much work
// TODO: SHOULD BE IN ANOTHER FILE, MAYBE EVEN ANOTHER PROJECT!!!
internal class MailService(IResend resend) : IEmailService
{
    public async Task SendAccountConfirmationMail(string recipientMail, string confirmationLink)
    {
        // todo: this html should be in a separate file
        var message = new EmailMessage
        {
            From = "support@econdo.online",
            Subject = "Account Confirmation",
            HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>ECondo Account Confirmation</title>
            </head>
            <body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f7f9fc; color: #333;"">
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); margin-top: 20px; margin-bottom: 20px;"">
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 30px 0; text-align: center; background-color: #0070f3; border-top-left-radius: 8px; border-top-right-radius: 8px;"">
                            <h1 style=""color: white; margin: 0; font-size: 24px;"">ECondo</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <h2 style=""margin-top: 0; color: #333; font-weight: 600;"">Welcome to ECondo!</h2>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                Thank you for registering with ECondo. We're excited to have you join our community of property managers and residents.
                            </p>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                To complete your registration and activate your account, please confirm your email address by clicking the button below:
                            </p>
                            
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href=""{confirmationLink}"" style=""background-color: #0070f3; color: white; padding: 12px 30px; text-decoration: none; border-radius: 4px; font-weight: 600; display: inline-block; font-size: 16px;"">Confirm Account</a>
                            </div>
                            
                            <div style=""font-size: 14px; color: #777; background-color: #f8f9fa; padding: 15px; border-radius: 4px; border-left: 4px solid #ffc107;"">
                                <p style=""margin: 0;"">
                                    <strong>Note:</strong> This confirmation link will expire in 48 hours. Please confirm your account before then to ensure access to all ECondo features.
                                </p>
                            </div>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555; margin-top: 25px;"">
                                Once your account is confirmed, you'll have access to:
                            </p>
                            
                            <ul style=""font-size: 16px; line-height: 1.6; color: #555; margin-top: 10px;"">
                                <li>Property management tools</li>
                                <li>Community announcements and updates</li>
                                <li>Maintenance request submissions</li>
                                <li>Secure document storage</li>
                            </ul>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555; margin-top: 25px;"">
                                If you did not create an account with ECondo, please disregard this email.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 20px; text-align: center; background-color: #f7f9fc; color: #888; font-size: 14px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; border-top: 1px solid #eaeaea;"">
                            <p style=""margin: 0;"">This is an automated message from ECondo. Please do not reply to this email.</p>
                            <p style=""margin: 10px 0 0 0;"">Need help? Contact our support team at support@econdo.online</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            "
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
            HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>ECondo Password Reset</title>
            </head>
            <body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f7f9fc; color: #333;"">
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); margin-top: 20px; margin-bottom: 20px;"">
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 30px 0; text-align: center; background-color: #0070f3; border-top-left-radius: 8px; border-top-right-radius: 8px;"">
                            <h1 style=""color: white; margin: 0; font-size: 24px;"">ECondo</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <h2 style=""margin-top: 0; color: #333; font-weight: 600;"">Password Reset Request</h2>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                We received a request to reset your password for your ECondo account. Security of your account is important to us, and we're here to help.
                            </p>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                To create a new password and regain access to your account, please click the button below:
                            </p>
                            
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href=""{resetLink}"" style=""background-color: #0070f3; color: white; padding: 12px 30px; text-decoration: none; border-radius: 4px; font-weight: 600; display: inline-block; font-size: 16px;"">Reset Password</a>
                            </div>
                            
                            <div style=""font-size: 14px; color: #777; background-color: #f8f9fa; padding: 15px; border-radius: 4px; border-left: 4px solid #ffc107;"">
                                <p style=""margin: 0;"">
                                    <strong>Important:</strong> This password reset link is only valid for 24 hours. After that, you'll need to request a new one.
                                </p>
                            </div>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555; margin-top: 25px;"">
                                If you didn't request a password reset, please disregard this email or contact our support team if you have concerns about your account security.
                            </p>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                For security reasons, this link can only be used once. After resetting your password, you'll be able to log in with your new credentials.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 20px; text-align: center; background-color: #f7f9fc; color: #888; font-size: 14px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; border-top: 1px solid #eaeaea;"">
                            <p style=""margin: 0;"">This is an automated message from ECondo. Please do not reply to this email.</p>
                            <p style=""margin: 10px 0 0 0;"">Need help? Contact our support team at support@econdo.online.</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            "
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
            HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>ECondo Invitation</title>
            </head>
            <body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f7f9fc; color: #333;"">
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); margin-top: 20px; margin-bottom: 20px;"">
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 30px 0; text-align: center; background-color: #0070f3; border-top-left-radius: 8px; border-top-right-radius: 8px;"">
                            <h1 style=""color: white; margin: 0; font-size: 24px;"">ECondo</h1>
                        </td>
                    </tr>
                    
                    <!-- Content -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            <h2 style=""margin-top: 0; color: #333; font-weight: 600;"">Hello, {firstName}!</h2>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                You've been added as an occupant in the ECondo system.
                            </p>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555;"">
                                Please click the button below to accept the invitation and connect your account:
                            </p>
                            
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href=""{invitationLink}"" style=""background-color: #0070f3; color: white; padding: 12px 30px; text-decoration: none; border-radius: 4px; font-weight: 600; display: inline-block; font-size: 16px;"">Accept Invitation</a>
                            </div>
                            
                            <div style=""font-size: 14px; color: #777; background-color: #f8f9fa; padding: 15px; border-radius: 4px; border-left: 4px solid #ffc107;"">
                                <p style=""margin: 0;"">
                                    <strong>Important:</strong> This invitation will expire on {expiresAt:f} UTC.
                                </p>
                            </div>
                            
                            <p style=""font-size: 16px; line-height: 1.6; color: #555; margin-top: 25px;"">
                                If you don't have an account yet, you will be able to register after clicking the link.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 20px; text-align: center; background-color: #f7f9fc; color: #888; font-size: 14px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; border-top: 1px solid #eaeaea;"">
                            <p style=""margin: 0;"">This is an automated message from ECondo. Please do not reply to this email.</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            "
        };

        message.To.Add(recipientMail);
        return resend.EmailSendAsync(message);
    }
}
