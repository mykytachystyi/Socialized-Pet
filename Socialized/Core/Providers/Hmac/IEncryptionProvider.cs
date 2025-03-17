namespace Core.Providers.Hmac;

public interface IEncryptionProvider
{
    SaltAndHash HashPassword(string password);
    bool VerifyPasswordHash(string password, SaltAndHash saltAndHash);
}
