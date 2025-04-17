using Serilog;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Core.SmtpMailing;

public class SmtpSender(ILogger logger, IOptions<MailSettings> mailSettings) : ISmtpSender
{
    private readonly MailSettings Settings = mailSettings.Value;
    private readonly ILogger Logger = logger;
    
    public void SendEmail(string email, string subject, string text)
    {
        var message = new MailMessage(Settings.MailAddress, email, subject, text);
        var client = new SmtpClient(Settings.Server, Settings.SmtpPort); 
        client.EnableSsl = Settings.SslOrTls;
        client.Credentials = new NetworkCredential(Settings.SmtpAddress, Settings.MailPassword);
        try
        {
            if (Settings.EnableWorking)
            {
                client.Send(message);
            }
            Logger.Information($"Був відправиленний лист на адресу={email}.");
        }
        catch (Exception ex)
        {

            Logger.Error($"Сервер не може відправити листа по адресі={email}");
            Logger.Error($"Виключення={ex.Message}");
            Logger.Error($"Внутрішне виключення={ex.InnerException?.Message}");
        }
    }
}