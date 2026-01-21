using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DeliFHery.API.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailOptions _opt;

        public EmailSenderService(IOptions<EmailOptions> opt)
        {
            _opt = opt.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string body, CancellationToken ct)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_opt.FromName, _opt.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };


            using var client = new SmtpClient();

            var secure = _opt.UseStarTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

            await client.ConnectAsync(_opt.Host, _opt.Port, secure, ct);
            await client.AuthenticateAsync(_opt.Username, _opt.Password, ct);
            await client.SendAsync(message,ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
