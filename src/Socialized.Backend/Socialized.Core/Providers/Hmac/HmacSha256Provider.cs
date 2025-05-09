﻿using System.Security.Cryptography;
using System.Text;

namespace Core.Providers.Hmac;

public class HmacSha256Provider : IEncryptionProvider
{
    public SaltAndHash HashPassword(string password)
    {
        using var hmac = new HMACSHA256();

        var salt = hmac.Key;

        var bytes = Encoding.UTF8.GetBytes(password);

        var hash = hmac.ComputeHash(bytes);

        return new SaltAndHash(salt, hash);
    }

    public bool VerifyPasswordHash(string password, SaltAndHash saltAndHash)
    {
        using var hmac = new HMACSHA256(saltAndHash.Salt);

        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var compute = hmac.ComputeHash(passwordBytes);

        return compute.SequenceEqual(saltAndHash.Hash);
    }
}