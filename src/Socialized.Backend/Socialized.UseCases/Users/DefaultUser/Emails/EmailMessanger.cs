using Core.SmtpMailing;

namespace UseCases.Users.DefaultUser.Emails
{
    public class EmailMessanger : IEmailMessanger
    {
        private ISmtpSender SmtpSender;

        public EmailMessanger(ISmtpSender smtpSender)
        {
            SmtpSender = smtpSender;
        }
        public void SendConfirmEmail(string userEmail, string culture, string userHash)
        {
            SmtpSender.SendEmail(userEmail, "DEV TEST CONFIRMATION ACCOUNT", userHash);
        }
        public void SendRecoveryEmail(string userEmail, string culture, int recoveryCode)
        {
            SmtpSender.SendEmail(userEmail, "DEV TEST RECOVERY PASSWORD", $"CODE:{recoveryCode}");          
        }
    }
}
