using Core.Providers;
using Core.Providers.Hmac;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.ChangeOldPassword;

public class ChangeOldPasswordCommandHandler (
    IUserRepository userRepository,
    IEncryptionProvider encryptionProvider,
    ILogger logger) : IRequestHandler<ChangeOldPasswordCommand, ChangeOldPasswordResponse>
{
    public async Task<ChangeOldPasswordResponse> Handle(ChangeOldPasswordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок зміни старого паролю на новий для користувача.");
        var user = userRepository.GetByUserTokenNotDeleted(request.UserToken);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по токену для зміни старого паролю користувача.");
        }
        if (!encryptionProvider.VerifyPasswordHash(request.NewPassword, 
            new SaltAndHash { Hash = user.HashedPassword, Salt = user.HashedSalt }))
        {
            throw new ValidationException("Пароль користувача не співпадає з паролем на сервері для заміни старого паролю.");
        }
        var newHashedPassword = encryptionProvider.HashPassword(request.NewPassword);
        user.HashedPassword = newHashedPassword.Hash;
        user.HashedSalt = newHashedPassword.Salt;
        userRepository.Update(user);
        logger.Information($"Старий пароль користувача було зміненно, id={user.Id}.");
        return new ChangeOldPasswordResponse(true, "Пароль було успішно зміненно.");
    }
}