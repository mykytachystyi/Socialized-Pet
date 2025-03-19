using Serilog;
using NSubstitute;
using UseCases.Admins.Emails;
using Core.SmtpMailing;

namespace UseCasesTests.Admins
{
    public class AdminEmailManagerTests
    {
        private readonly ISmtpSender smtpSender;
        private readonly ILogger logger;
        private readonly AdminEmailManager adminEmailManager;

        public AdminEmailManagerTests()
        {
            smtpSender = Substitute.For<ISmtpSender>();
            logger = Substitute.For<ILogger>();
            adminEmailManager = new AdminEmailManager(smtpSender, logger);
        }

        [Fact]
        public void SetupPassword_SendsEmailAndLogsInformation()
        {
            // Arrange
            var tokenForStart = "sample_token";
            var email = "test@test.com";

            // Act
            adminEmailManager.SetupPassword(tokenForStart, email);

            // Assert
            smtpSender.Received().SendEmail(email, "Створення паролю", tokenForStart);
            logger.Received().Information("Був відправлений URL для активації вашого адмін аккаунту.");
        }

        [Fact]
        public void RecoveryPassword_SendsEmailAndLogsInformation()
        {
            // Arrange
            var code = 123456;
            var email = "test@test.com";

            // Act
            adminEmailManager.RecoveryPassword(code, email);

            // Assert
            smtpSender.Received().SendEmail(email, "Відновлення паролю", $"Code: {code}");
            logger.Received().Information("Був відправлений 6 знаковий код для відновлення паролю.");
        }
    }
}
