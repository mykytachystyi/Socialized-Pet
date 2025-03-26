namespace Core.Providers.TextEncrypt;

public interface ITextEncryptionProvider
{
    public string Encrypt(string clearText);
    public string Decrypt(string cipherText);
}
