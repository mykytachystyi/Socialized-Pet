namespace Core.Providers;

public record struct SaltAndHash(
    byte[] Salt,
    byte[] Hash
);