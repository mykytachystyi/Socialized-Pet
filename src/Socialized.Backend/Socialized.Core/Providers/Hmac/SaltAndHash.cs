namespace Core.Providers.Hmac;

public record struct SaltAndHash(
    byte[] Salt,
    byte[] Hash
);