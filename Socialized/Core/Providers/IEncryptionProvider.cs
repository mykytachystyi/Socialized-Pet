namespace Core.Providers;

public interface IEncryptionProvider
{
    SaltAndHash HashPassword(string password);
    bool VerifyPasswordHash(string password, SaltAndHash saltAndHash);
}
