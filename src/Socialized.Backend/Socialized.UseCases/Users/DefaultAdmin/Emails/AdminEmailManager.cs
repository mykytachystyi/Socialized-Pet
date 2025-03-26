using Core.SmtpMailing;
using Serilog;

namespace UseCases.Users.DefaultAdmin.Emails;

public class AdminEmailManager : IAdminEmailManager
{
    private ISmtpSender SmtpSender;
    private ILogger Logger;
    public AdminEmailManager(ISmtpSender smtpSender, ILogger logger)
    {
        SmtpSender = smtpSender;
        Logger = logger;
    }
    public void SetupPassword(string tokenForStart, string email)
    {
        SmtpSender.SendEmail(email, "Створення паролю", tokenForStart);
        Logger.Information("Був відправлений URL для активації вашого адмін аккаунту.");
    }
    public void RecoveryPassword(int code, string email)
    {
        SmtpSender.SendEmail(email, "Відновлення паролю", $"Code: {code}");
        Logger.Information("Був відправлений 6 знаковий код для відновлення паролю.");
    }
}