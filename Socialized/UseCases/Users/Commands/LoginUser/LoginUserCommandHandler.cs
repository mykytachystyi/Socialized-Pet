using AutoMapper;
using Core;
using Domain.Users;
using MediatR;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Models;

namespace UseCases.Users.Commands.LoginUser;

public class LoginUserCommandHandler (
    IUserRepository userRepository,
    ProfileCondition profileCondition,
    IMapper mapper,
    ILogger logger
    ) : IRequestHandler<LoginUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(LoginUserCommand request, 
        CancellationToken cancellationToken)
    {
        logger.Information($"Початок входу(логіну) користувача, email={request.Email}.");
        var user = userRepository.GetByEmail(request.Email);
        if (user == null)
        {
            throw new NotFoundException("Сервер не визначив користувача по email для логіну.");
        }
        if (!profileCondition.VerifyHashedPassword(user.Password, request.Password))
        {
            throw new ValidationException("Пароль користувача не співпадає з паролем на сервері.");
        }
        user.LastLoginAt = DateTime.UtcNow;
        userRepository.Update(user);
        logger.Information($"Користувач був залогінен, id={user.Id}.");
        return mapper.Map<UserResponse>(user);
    }
}