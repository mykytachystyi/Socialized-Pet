﻿namespace UseCases.Users.DefaultUser.Emails
{
    public interface IEmailMessanger
    {
        void SendConfirmEmail(string userEmail, string culture, string userHash);
        void SendRecoveryEmail(string userEmail, string culture, int recoveryCode);
    }
}
