using Core.Providers.Hmac;
using Domain.Users;
using FluentAssertions;
using Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Users.Commands.Activate;
using UseCases.Users.Commands.ChangeOldPassword;
using UseCases.Users.Commands.ChangePassword;
using UseCases.Users.Commands.CheckRecoveryCode;
using UseCases.Users.Commands.CreateUser;
using UseCases.Users.Commands.Delete;
using UseCases.Users.Commands.LoginUser;
using UseCases.Users.Commands.LogOut;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Commands.RegistrationEmail;
using UseCases.Users.Models;

namespace WebApiCompose.IntegrationTests.Controllers
{
    public class UsersControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task Registration_ReturnOk()
        {
            // Arrange
            var command = new CreateUserCommand()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "testing@example.com",
                Password = "Pass1234!",
                CountryName = "United States",
                TimeZone = 0,
                Culture = "en-US"
            };

            // Act
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Users/Registration/", content);
            var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task RegistrationEmail_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();

            // Act
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            var response = await Client.GetAsync($"/1.0/Users/RegistrationEmail?email={user.Email}");
            var result = await response.Content.ReadFromJsonAsync<RegistrationEmailResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();

        }
        [Fact]
        public async Task Login_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();

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
            var response = await Client.PostAsync("/1.0/Users/Login/", content);
            var result = await response.Content.ReadFromJsonAsync<UserResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Email.Should().Be(user.Email);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
        }
        [Fact]
        public async Task LogOut_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var command = new LogOutCommand
            {
                UserToken = user.TokenForUse
            };
            // Act
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Users/LogOut/", content);
            var result = await response.Content.ReadFromJsonAsync<LogOutResponse>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task RecoveryPassword_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();

            // Act
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            var response = await Client.GetAsync($"/1.0/Users/RecoveryPassword?email={user.Email}");
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task CheckRecoveryCode_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();

            // Act
            var command = new CheckRecoveryCodeCommand
            {
                UserEmail = user.Email,
                RecoveryCode = user.RecoveryCode.Value
            };
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Users/CheckRecoveryCode/", content);
            var result = await response.Content.ReadFromJsonAsync<CheckRecoveryCodeResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.RecoveryToken.Should().NotBeEmpty();
        }
        [Fact]
        public async Task ChangePassword_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = user.RecoveryToken,
                UserPassword = "Pass1234!",
                UserConfirmPassword = "Pass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Client.PostAsync("/1.0/Users/ChangePassword/", content);
            var result = await response.Content.ReadFromJsonAsync<ChangeUserPasswordResponse>();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task ChangeOldPassword_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var command = new ChangeOldPasswordCommand
            {
                UserToken = user.TokenForUse,
                OldPassword = "Pass1234!",
                NewPassword = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Client.PostAsync("/1.0/Users/ChangeOldPassword/", content);
            var result = await response.Content.ReadFromJsonAsync<ChangeOldPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Activate_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();

            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            user.Activate = false;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            // Act
            var response = await Client.GetAsync($"/1.0/Users/Activate?hash={user.HashForActivate}");
            var result = await response.Content.ReadFromJsonAsync<ActivateResponse>();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Delete_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var command = new DeleteCommand
            {
                UserToken = user.TokenForUse
            };
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Client.PostAsync($"/1.0/Users/Delete/", content);
            var result = await response.Content.ReadFromJsonAsync<DeleteResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        private async Task<User> CreateTestUser()
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var hashedPassword = new HmacSha256Provider().HashPassword("Pass1234!");

            var user = new User
            {
                FirstName = "Rick",
                LastName = "Cross",
                Email = "user@example.com",
                HashedPassword = hashedPassword.Hash,
                HashedSalt = hashedPassword.Salt,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTimeOffset.UtcNow,
                RecoveryCode = 123456,
                RecoveryToken = "RecoveryToken",
                TokenForUse = "TokenForUser",
                HashForActivate = "HashForActivate",
                Activate = true
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
