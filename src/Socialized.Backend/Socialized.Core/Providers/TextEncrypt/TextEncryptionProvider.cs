using System.Text;
using System.Security.Cryptography;

namespace Core.Providers.TextEncrypt;

public class TextEncryptionProvider : ITextEncryptionProvider
{
    private readonly string sum_names = "abc123";

    public string Encrypt(string clearText)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (var encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(
                sum_names,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 },
                100_000,
                HashAlgorithmName.SHA256
            );

            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public string Decrypt(string cipherText)
    {
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (var encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(
                sum_names,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 },
                100_000,
                HashAlgorithmName.SHA256
            );

            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

}
