using Core;
using MediatR;
using Serilog;
using Domain.Users;
using UseCases.Exceptions;
using Core.Providers.Hmac;

namespace UseCases.Users.Commands.ChangePassword;

public class ChangeUserPasswordCommandHandler (
    IUserRepository userRepository,
    IEncryptionProvider encryptionProvider,
    ILogger logger) : IRequestHandler<ChangeUserPasswordCommand, 
    ChangeUserPasswordResponse>
{
    public async Task<ChangeUserPasswordResponse> Handle(ChangeUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок зміни паролю для користувача.");
        var user = userRepository.GetByRecoveryToken(request.RecoveryToken, false);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по токену востановлення для зміни паролю.");
        }
        if (!request.UserPassword.Equals(request.UserConfirmPassword))
        {
            throw new ValidationException("Паролі не співпадають одне з одним.");
        }
        var newHashedPassword = encryptionProvider.HashPassword(request.UserPassword);
        user.HashedPassword = newHashedPassword.Hash;
        user.HashedSalt = newHashedPassword.Salt;
        user.RecoveryToken = "";
        userRepository.Update(user);
        logger.Information($"Пароль користувача було зміненно, id={user.Id}.");
        return new ChangeUserPasswordResponse(true, "Пароль було зміненно.");
    }
}