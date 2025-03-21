namespace UseCases.Users.DefaultAdmin.Emails
{
    public interface IAdminEmailManager
    {
        void SetupPassword(string tokenForStart, string email);
        void RecoveryPassword(int code, string email);
    }
}
