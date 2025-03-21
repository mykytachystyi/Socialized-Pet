using Core.Providers.Hmac;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.DefaultUser.Commands.ChangeOldPassword;

public class ChangeOldPasswordCommandHandler(
    IRepository<User> userRepository,
    IEncryptionProvider encryptionProvider,
    ILogger logger) : IRequestHandler<ChangeOldPasswordWithUserCommand, ChangeOldPasswordResponse>
{
    public async Task<ChangeOldPasswordResponse> Handle(ChangeOldPasswordWithUserCommand request,
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок зміни старого паролю на новий для користувача.");

        var user = await userRepository.FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по токену для зміни старого паролю користувача.");
        }
        if (!encryptionProvider.VerifyPasswordHash(request.OldPassword,
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