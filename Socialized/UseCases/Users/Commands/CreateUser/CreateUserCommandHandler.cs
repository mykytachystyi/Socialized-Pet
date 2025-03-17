using Core.Providers.Hmac;
using Core.Providers.Rand;
using Domain.Users;
using MediatR;
using Serilog;
using System.Web;
using UseCases.Exceptions;
using UseCases.Users.Emails;

namespace UseCases.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler (
        IUserRepository userRepository,
        IEmailMessanger emailMessanger,
        IEncryptionProvider encryptionProvider,
        IRandomizer randomizer,
        ILogger logger
        ) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            logger.Information("Початок створення нового користувача.");
            var user = userRepository.GetByEmail(request.Email);
            if (user != null && user.IsDeleted)
            {
                user.IsDeleted = false;
                user.TokenForUse = Guid.NewGuid().ToString();
                userRepository.Update(user);
                logger.Information($"Був відновлен видалений аккаунт, id={user.Id}.");
                return new CreateUserResponse(true, $"Був відновлен видалений аккаунт, id={user.Id}.");
            }
            if (user != null && !user.IsDeleted)
            {
                throw new NotFoundException("Користувач з таким email-адресом вже існує.");
            }
            var hashedPasswordPair = encryptionProvider.HashPassword(request.Password);
            user = new User
            {
                Email = request.Email,
                FirstName = HttpUtility.UrlDecode(request.FirstName),
                LastName = HttpUtility.UrlDecode(request.LastName),
                HashedPassword = hashedPasswordPair.Hash,
                HashedSalt = hashedPasswordPair.Salt,
                HashForActivate = randomizer.CreateHash(100),
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                TokenForUse = randomizer.CreateHash(40),
                RecoveryToken = ""
            };
            userRepository.Create(user);
            emailMessanger.SendConfirmEmail(user.Email, request.Culture, user.HashForActivate);
            logger.Information($"Новий користувач був створений, id={user.Id}.");
            return new CreateUserResponse(true, $"Новий користувач був створений, id={user.Id}.");
        }
    }
}