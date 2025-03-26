using Domain.Appeals;
using Domain.Users;
using Infrastructure;
using System.Text;
using System.Text.Json;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using UseCases.Users.DefaultUser.Models;

namespace WebApiCompose.IntegrationTests
{
    public class IntegrationTestContext : IntegrationTestBase
    {
        public User ActualUser;
        public string ActualUserJwtToken;

        public virtual async Task<string> SetupUser(User user)
        {
            var command = new LoginUserCommand
            {
                Email = user.Email,
                Password = "Pass1234!"
            };
            // Act
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = Client.PostAsync("/1.0/Users/Login/", content);
            var result = response.Result.Content.ReadFromJsonAsync<LoginTokenResponse>();

            ActualUserJwtToken = result!.Result!.AuthenticationToken;
            return result!.Result!.AuthenticationToken;
        }
        public virtual async Task<string> SetupAdmin(User user)
        {
            var command = new LoginUserCommand
            {
                Email = user.Email,
                Password = "Pass1234!"
            };
            // Act
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Admins/Login/", content);
            var result = await response.Content.ReadFromJsonAsync<LoginTokenResponse>();

            return result!.AuthenticationToken;
        }
        public virtual async Task<User> CreateTestUser()
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = context.Users.FirstOrDefault(u => u.Email == "user@example.com");

            ActualUser = user;
            return user;
        }
        public async Task<User> CreateTestAdmin()
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var admin = context.Users.FirstOrDefault(u => u.Email == "admin@example.com");
            return admin;
        }
        public async Task<Appeal> CreateAppeal(User user)
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var appeal = new Appeal
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = DateTimeOffset.UtcNow,
                Subject = "Test subject",
                LastActivity = DateTimeOffset.UtcNow,
            };
            context.Appeals.Add(appeal);
            await context.SaveChangesAsync();
            return appeal;
        }
        public async Task<AppealMessage> CreateMessage(Appeal appeal)
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var appealMessage = new AppealMessage
            {
                AppealId = appeal.Id,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = DateTimeOffset.UtcNow,
                Message = "Test message",
                IsDeleted = false,
                LastUpdatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };
            context.AppealMessages.Add(appealMessage);
            await context.SaveChangesAsync();

            return appealMessage;
        }
    }
}
