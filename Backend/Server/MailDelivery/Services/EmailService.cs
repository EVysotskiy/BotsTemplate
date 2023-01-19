using MailDelivery.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailDelivery.Services;

public interface IEmailService
{
    Task SendEmail(string email, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailConfig)
    {
        _emailOptions = emailConfig.Value;
    }

    public async Task SendEmail(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("Администрация сайта", _emailOptions.UserName));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, _emailOptions.IsUseSsl);
            await client.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password);
            await client.SendAsync(emailMessage);
 
            await client.DisconnectAsync(true);
        }
    }
}