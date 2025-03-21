using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Exceptions;
using UseCases.Users.DefaultAdmin.Commands.DeleteAdmin;
using UseCases.Users.DefaultAdmin.Commands.SetupPassword;
using UseCases.Users.DefaultAdmin.Models;
using UseCases.Users.DefaultUser.Commands.ChangeOldPassword;
using UseCases.Users.DefaultUser.Commands.ChangePassword;
using UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;
using UseCases.Users.DefaultUser.Commands.CreateUser;
using UseCases.Users.DefaultUser.Commands.LoginUser;
using UseCases.Users.DefaultUser.Commands.RecoveryPassword;
using UseCases.Users.DefaultUser.Models;

namespace WebApiCompose.IntegrationTests.Controllers
{
    public class AdminControllerTests : UsersControllerTests
    {
        [Fact]
        public async Task LoginAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();

            // Act
            var content = new StringContent(JsonSerializer.Serialize(
                new LoginUserCommand
                {
                    Email = admin.Email,
                    Password = "Pass1234!"
                }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Admins/Login/", content);

            var result = await response.Content.ReadFromJsonAsync<LoginTokenResponse>();


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.AuthenticationToken.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task LoginAdmin_WhenPasswordIsWrong_ReturnErrorMessage()
        {
            // Arrange
            var admin = await CreateTestAdmin();

            // Act
            var content = new StringContent(JsonSerializer.Serialize(
                new LoginUserCommand
                {
                    Email = admin.Email,
                    Password = "WrongPassword1234"
                }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Admins/Login/", content);

            var result = await response.Content.ReadFromJsonAsync<ValidationException>();


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            result!.Message.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task CreateAdmin_ReturnNewAdminResponse()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);

            var command = new CreateUserCommand
            {
                Email = "newAdmin@example.com",
                Password = "Pass1234!",
                FirstName = "Nick",
                LastName = "Cross"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.PostAsync("/1.0/Admins/Create/", content);
            var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task SetupPassword_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);

            var command = new SetupPasswordCommand
            {
                Password = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.PostAsync("/1.0/Admins/SetupPassword/", content);
            var result = await response.Content.ReadFromJsonAsync<SetupPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.DeleteAsync($"/1.0/Admins/Delete?adminId={admin.Id}" );
            var result = await response.Content.ReadFromJsonAsync<DeleteAdminResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task RecoveryPasswordAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();

            // Act
            var response = await Client.GetAsync($"/1.0/Admins/RecoveryPassword?email={admin.Email}");
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task CheckRecoveryCodeAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var command = new CheckRecoveryCodeCommand
            {
                Email = admin.Email,
                RecoveryCode = admin.RecoveryCode!.Value
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync($"/1.0/Admins/CheckRecoveryCode/", content);
            var result = await response.Content.ReadFromJsonAsync<CheckRecoveryCodeResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.RecoveryToken.Should().NotBeEmpty();
        }
        [Fact]
        public async Task ChangePasswordAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = admin.RecoveryToken,
                UserPassword = "NewPass1234!",
                UserConfirmPassword = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync($"/1.0/Admins/ChangePassword/", content);
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task ChangeOldPasswordAdmin_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);
            var command = new ChangeOldPasswordCommand
            {
                OldPassword = "Pass1234!",
                NewPassword = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Act
            var response = await Client.PostAsync($"/1.0/Admins/ChangeOldPassword/", content);
            var result = await response.Content.ReadFromJsonAsync<ChangeOldPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Success.Should().BeTrue();
        }
        [Fact]
        public async Task GetAdmins_ReturnListOfAdmins()
        {
            // Arrange
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.GetAsync($"/1.0/Admins/GetAdmins?since=0&count=10");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Count().Should().Be(0);
        }
        [Fact]
        public async Task GetUsers_ReturnListOfUsers()
        {
            // Arrange
            await CreateTestUser();
            var admin = await CreateTestAdmin();
            var token = await SetupAdmin(admin);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.GetAsync("/1.0/Admins/GetUsers/?since=0&count=10");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Count().Should().BeGreaterThan(0);
        }
    }
}