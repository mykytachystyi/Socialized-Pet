using Core;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;

namespace UseCases.Users.Commands.ChangeOldPassword;

public class ChangeOldPasswordCommandHandler (
    IUserRepository userRepository,
    ProfileCondition profileCondition,
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
        if (!profileCondition.VerifyHashedPassword(user.Password, request.OldPassword))
        {
            throw new ValidationException("Пароль користувача не співпадає з паролем на сервері для заміни старого паролю.");
        }
        user.Password = profileCondition.HashPassword(request.NewPassword);
        userRepository.Update(user);
        logger.Information($"Старий пароль користувача було зміненно, id={user.Id}.");
        return new ChangeOldPasswordResponse(true, "Пароль було успішно зміненно.");
    }
}