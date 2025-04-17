using Serilog;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Core.SmtpMailing
{
    public class SmtpOauthSender : ISmtpSender
    {
        private readonly MailSettings Settings;
        private readonly ILogger Logger;

        public SmtpOauthSender(ILogger logger, IOptions<MailSettings> mailSettings)
        {
            Logger = logger;
            Settings = mailSettings.Value;
        }

        public void SendEmail(string email, string subject, string text)
        {
            var secureSocketOptions = Settings.SslOrTls ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Settings.SmtpAddress, Settings.MailAddress));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = text };
            using (var client = new SmtpClient())
            {
                client.ConnectAsync(Settings.SmtpAddress, Settings.SmtpPort, secureSocketOptions);

                var oauth2 = new SaslMechanismOAuth2(Settings.MailAddress, "");
                client.AuthenticateAsync(oauth2);

                client.SendAsync(message);
                client.DisconnectAsync(true);
            }

            Logger.Information($"Був відправлений лист на адресу={email}.");
        }
    }
}
