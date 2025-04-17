namespace Core.SmtpMailing
{
    public class MailSettings
    {
        public required string Server { get; set; }
        public required string MailAddress { get; set; }
        public required string MailPassword { get; set; }
        public required string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
        public bool SslOrTls { get; set; }
        public bool EnableWorking { get; set; }
    }
}
