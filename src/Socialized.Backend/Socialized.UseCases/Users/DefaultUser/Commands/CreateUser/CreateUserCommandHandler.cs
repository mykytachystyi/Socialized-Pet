using Core.Providers.Hmac;
using Core.Providers.Rand;
using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories;
using MediatR;
using Serilog;
using System.Web;
using UseCases.Exceptions;
using UseCases.Users.DefaultUser.Emails;

namespace UseCases.Users.DefaultUser.Commands.CreateUser
{
    public class CreateUserCommandHandler(
        IRepository<User> userRepository,
        IEmailMessanger emailMessanger,
        IEncryptionProvider encryptionProvider,
        IRandomizer randomizer,
        ILogger logger
        ) : IRequestHandler<CreateUserWithRoleCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserWithRoleCommand command, CancellationToken cancellationToken)
        {
            logger.Information("Початок створення нового користувача.");

            var user = await userRepository.FirstOrDefaultAsync(u => u.Email == command.Email);
            if (user != null && user.IsDeleted)
            {
                user.IsDeleted = false;
                userRepository.Update(user);
                logger.Information($"Був відновлен видалений аккаунт, id={user.Id}.");
                return new CreateUserResponse(true, $"Був відновлен видалений аккаунт, id={user.Id}.");
            }
            if (user != null && !user.IsDeleted)
            {
                throw new NotFoundException("Користувач з таким email-адресом вже існує.");
            }
            var hashedPasswordPair = encryptionProvider.HashPassword(command.Password);
            user = new User
            {
                Email = command.Email,
                FirstName = HttpUtility.UrlDecode(command.FirstName),
                LastName = HttpUtility.UrlDecode(command.LastName),
                HashedPassword = hashedPasswordPair.Hash,
                HashedSalt = hashedPasswordPair.Salt,
                HashForActivate = randomizer.CreateHash(100),
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                RecoveryToken = "",
                Role = command.Role
            };
            await userRepository.AddAsync(user);

            if (command.Role == (int)IdentityRole.DefaultUser)
            {
                emailMessanger.SendConfirmEmail(user.Email, command.Culture, user.HashForActivate);
            }
            logger.Information($"Новий користувач був створений, id={user.Id}.");
            return new CreateUserResponse(true, $"Новий користувач був створений, id={user.Id}.");
        }
    }
}