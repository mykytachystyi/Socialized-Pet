using Core.Providers.Hmac;
using Domain.Admins;
using Domain.Appeals;
using Domain.Users;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Admins.Commands.Authentication;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;
using WebApiCompose.Responses;

namespace WebApiCompose.IntegrationTests.Controllers.Appeals;

public class AppealsControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_ReturnOk()
    {
        // Arrange
        var user = await CreateTestUser();
        var content = new StringContent(JsonSerializer.Serialize(
            new CreateAppealCommand
            {
                Subject = "Test subject",
                UserToken = user.TokenForUse,
            }),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await Client.PostAsync("/1.0/Appeals/Create/", content);
        var result = await response.Content.ReadFromJsonAsync<AppealResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var appeal = await context.Appeals.FirstOrDefaultAsync(t => t.UserId == user.Id);
        appeal.Should().NotBeNull();
        appeal!.Subject.Should().Be("Test subject");
    }
    [Fact]
    public async Task GetAppealsByUser_ReturnCollectionAppeals()
    {
        // Arrange
        var user = await CreateTestUser();
        await CreateTestAppeal(user);

        // Act
        var response = await Client.GetAsync($"/1.0/Appeals/GetAppealsByUser?userToken={user.TokenForUse}&since=0&count=10");
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppealResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Count().Should().Be(3);
    }
    [Fact]
    public async Task GetAppealsByAdmin_ReturnCollectionAppeals()
    {
        // Arrange
        var user = await CreateTestUser();
        await CreateTestAppeal(user);
        var contentAdmin = new StringContent(JsonSerializer.Serialize(
                new AuthenticationCommand
                {
                    Email = "user@example.com",
                    Password = "Pass1234!"
                }),
                Encoding.UTF8,
                "application/json"
            );
        var responseAdmin = await Client.PostAsync("/1.0/Admins/Authentication/", contentAdmin);
        var resultAdmin = await responseAdmin.Content.ReadFromJsonAsync<AdminTokenResponse>();

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resultAdmin.AdminToken);
        var response = await Client.GetAsync($"/1.0/Appeals/GetAppealsByAdmin?since=0&count=10");
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppealResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Count().Should().Be(3);
    }
    public async Task<User> CreateTestUser()
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
    public async Task CreateTestAppeal(User user)
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        for (int i = 0; i < 3; i++)
        {
            context.Appeals.Add(new Appeal
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = DateTimeOffset.UtcNow,
                Subject = "Test subject",
                LastActivity = DateTimeOffset.UtcNow,
            });
            await context.SaveChangesAsync();
        }
    }
}
