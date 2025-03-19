using Core.Providers.Hmac;
using Domain.Admins;
using Domain.Users;
using FluentAssertions;
using Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Admins.Commands.Authentication;
using UseCases.Admins.Commands.ChangePassword;
using UseCases.Admins.Commands.CreateAdmin;
using UseCases.Admins.Commands.Delete;
using UseCases.Admins.Commands.SetupPassword;
using UseCases.Admins.Models;
using UseCases.Exceptions;
using UseCases.Users.Commands.RecoveryPassword;
using UseCases.Users.Models;
using WebApiCompose.Responses;

namespace WebApiCompose.IntegrationTests.Controllers
{
    public class AdminControllerTests : IntegrationTestBase
    {
        public Admin ActualAdmin { get; set; }
        public string ActualJwtToken { get; set; }
        [Fact]
        public async Task Authentication_ReturnOk()
        {
            // Arrange
            var admin = await CreateTestAdmin();

            // Act
            var content = new StringContent(JsonSerializer.Serialize(
                new AuthenticationCommand
                {
                    Email = admin.Email,
                    Password = "Pass1234!"
                }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Admins/Authentication/", content);

            var result = await response.Content.ReadFromJsonAsync<AdminTokenResponse>();


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result!.AdminToken.Should().NotBeNullOrEmpty();
            ActualJwtToken = result.AdminToken;
        }
        [Fact]
        public async Task Authentication_WhenPasswordIsWrong_ReturnErrorMessage()
        {
            // Arrange
            var admin = await CreateTestAdmin();

            // Act
            var content = new StringContent(JsonSerializer.Serialize(
                new AuthenticationCommand
                {
                    Email = admin.Email,
                    Password = "WrongPassword1234"
                }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync("/1.0/Admins/Authentication/", content);

            var result = await response.Content.ReadFromJsonAsync<ValidationException>();


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            result!.Message.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task Create_ReturnNewAdminResponse()
        {
            // Arrange
            await Authentication_ReturnOk();

            var command = new CreateAdminCommand
            {
                Email = "newAdmin@example.com",
                Password = "Pass1234!",
                FirstName = "Nick",
                LastName = "Cross"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualJwtToken);

            // Act
            var response = await Client.PostAsync("/1.0/Admins/Create/", content);
            var result = await response.Content.ReadFromJsonAsync<AdminResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Email.Should().Be(command.Email);
            result.FirstName.Should().Be(command.FirstName);
            result.LastName.Should().Be(command.LastName);
        }
        [Fact]
        public async Task SetupPassword_ReturnOk()
        {
            // Arrange
            await Authentication_ReturnOk();

            var command = new SetupPasswordCommand
            {
                Token = "TokenForStart",
                Password = "NewPass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/1.0/Admins/SetupPassword/", content);
            var result = await response.Content.ReadFromJsonAsync<SetupPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Delete_ReturnOk()
        {
            // Arrange
            await Authentication_ReturnOk();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualJwtToken);

            // Act
            var response = await Client.DeleteAsync($"/1.0/Admins/Delete?adminId={ActualAdmin.Id}" );
            var result = await response.Content.ReadFromJsonAsync<DeleteAdminResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task RecoveryPassword_ReturnOk()
        {
            // Arrange
            await Authentication_ReturnOk();

            // Act
            var response = await Client.GetAsync($"/1.0/Admins/RecoveryPassword?adminEmail={ActualAdmin.Email}");
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task ChangePassword_ReturnOk()
        {
            // Arrange
            await Authentication_ReturnOk();
            var command = new ChangePasswordCommand
            {
                RecoveryCode = 123456,
                Password = "Pass1234!"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync($"/1.0/Admins/ChangePassword/", content);
            var result = await response.Content.ReadFromJsonAsync<RecoveryPasswordResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task GetAdmins_ReturnListOfAdmins()
        {
            // Arrange
            await Authentication_ReturnOk();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualJwtToken);

            // Act
            var response = await Client.GetAsync($"/1.0/Admins/GetAdmins?since=0&count=10");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<AdminResponse>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Count().Should().BeGreaterThan(0);
        }
        [Fact]
        public async Task GetUsers_ReturnListOfUsers()
        {
            // Arrange
            await CreateTestUser();
            await Authentication_ReturnOk();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualJwtToken);

            // Act
            var response = await Client.GetAsync($"/1.0/Admins/GetUsers?since=0&count=10");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Count().Should().BeGreaterThan(0);
        }
        private async Task<Admin> CreateTestAdmin()
        {
            using var scope = Application.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var hashedPassword = new HmacSha256Provider().HashPassword("Pass1234!");

            var admin = new Admin
            {
                FirstName = "Rick",
                LastName = "Cross",
                Email = "user@example.com",
                HashedPassword = hashedPassword.Hash,
                HashedSalt = hashedPassword.Salt,
                Role = "default",
                CreatedAt = DateTime.UtcNow,
                TokenForStart = "TokenForStart",
                LastLoginAt = DateTimeOffset.UtcNow,
                RecoveryCode = 123456
            };
            context.Admins.Add(admin);
            await context.SaveChangesAsync();

            ActualAdmin = admin;
            return admin;
        }
        private async Task CreateTestUser()
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
        }
    }
}