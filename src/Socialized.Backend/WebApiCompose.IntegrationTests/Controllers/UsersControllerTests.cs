using Domain.Users;
using FluentAssertions;
using Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Users.DefaultUser.Commands.Activate;
using UseCases.Users.DefaultUser.Commands.ChangeOldPassword;
using UseCases.Users.DefaultUser.Commands.ChangePassword;
using UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;
using UseCases.Users.DefaultUser.Commands.CreateUser;
using UseCases.Users.DefaultUser.Commands.Delete;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using UseCases.Users.DefaultUser.Commands.RecoveryPassword;
using UseCases.Users.DefaultUser.Commands.RegistrationEmail;
using UseCases.Users.DefaultUser.Models;

namespace WebApiCompose.IntegrationTests.Controllers
{
    public class UsersControllerTests : IntegrationTestContext
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
                Password = "Pass1234!"
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
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task RegistrationEmail_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);

            // Act
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            var response = await Client.GetAsync($"/1.0/Users/RegistrationEmail?email={ActualUser.Email}");
            var result = await response.Content.ReadFromJsonAsync<RegistrationEmailResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();

        }
        [Fact]
        public async Task Login_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);

            var command = new LoginUserCommand
            {
                Email = ActualUser.Email,
                Password = "Pass1234!"
            };
            // Act
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Users/Login/", content);
            var result = await response.Content.ReadFromJsonAsync<LoginTokenResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.AuthenticationToken.Should().NotBeEmpty();
        }
        [Fact]
        public async Task RecoveryPassword_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);

            // Act
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            var response = await Client.GetAsync($"/1.0/Users/RecoveryPassword?email={ActualUser.Email}");
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();

            // Verify database
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userFromDb = context.Users.First(u => u.Email == ActualUser.Email);
            userFromDb.RecoveryCode.Should().NotBeNull();
        }
        [Fact]
        public async Task CheckRecoveryCode_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);
            var command = new CheckRecoveryCodeCommand
            {
                Email = ActualUser.Email,
                RecoveryCode = ActualUser.RecoveryCode!.Value
            };
            var content = new StringContent(JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await Client.PostAsync("/1.0/Users/CheckRecoveryCode/", content);
            var result = await response.Content.ReadFromJsonAsync<CheckRecoveryCodeResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.RecoveryToken.Should().NotBeEmpty();
        }
        [Fact]
        public async Task ChangePassword_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = ActualUser.RecoveryToken,
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
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task ChangeOldPassword_ReturnOk()
        {
            // Arrange
            await Login_ReturnOk();
            var command = new ChangeOldPasswordCommand
            {
                OldPassword = "Pass1234!",
                NewPassword = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualUserJwtToken);

            // Act
            var response = await Client.PostAsync("/1.0/Users/ChangeOldPassword/", content);
            var result = await response.Content.ReadFromJsonAsync<ChangeOldPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Activate_ReturnOk()
        {
            // Arrange
            var user = await CreateTestUser();
            var token = await SetupUser(user);

            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            ActualUser.Activate = false;
            context.Users.Update(ActualUser);
            await context.SaveChangesAsync();

            // Act
            var response = await Client.GetAsync($"/1.0/Users/Activate?hash={ActualUser.HashForActivate}");
            var result = await response.Content.ReadFromJsonAsync<ActivateResponse>();
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Delete_ReturnOk()
        {
            // Arrange
            await Login_ReturnOk();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualUserJwtToken);

            // Act
            var response = await Client.DeleteAsync($"/1.0/Users/Delete/");
            var result = await response.Content.ReadFromJsonAsync<DeleteResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
    }
}
