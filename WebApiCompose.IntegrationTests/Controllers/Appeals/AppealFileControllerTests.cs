using Core.Providers.Hmac;
using Domain.Appeals;
using Domain.Users;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApiCompose.IntegrationTests.Controllers.Appeals;

public class AppealFileControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_ReturnOk()
    {
        // Arrange
        var message = await CreateNewUserWithAppealAndMessage();
        var content = new MultipartFormDataContent();
        var jsonContent = JsonSerializer.Serialize(new AppealMessage { Message = "Test message" });
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(jsonContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "files", "message.json");
        content.Add(fileContent, "files", "message.json");

        // Act
        var response = await Client.PostAsync($"/1.0/AppealFile/Create?messageId={message.Id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database 
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var file = await context.AppealFiles.FirstOrDefaultAsync(t => t.MessageId == message.Id);
        file.Should().NotBeNull();
        file!.MessageId.Should().Be(message.Id);
        file!.RelativePath.Should().NotBeNullOrEmpty();
    }
    public async Task<AppealMessage> CreateNewUserWithAppealAndMessage()
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

        var appeal = new Appeal
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = DateTimeOffset.UtcNow,
            Subject = "Test subject",
            LastActivity = DateTimeOffset.UtcNow,
            State = 0,
            IsDeleted = false,
            LastUpdatedAt = DateTimeOffset.UtcNow,
        };

        context.Appeals.Add(appeal);
        await context.SaveChangesAsync();

        var message = new AppealMessage
        {
            AppealId = appeal.Id,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = DateTimeOffset.UtcNow,
            Message = "Test message",
            IsDeleted = false,
            LastUpdatedAt = DateTimeOffset.UtcNow,
        };

        context.AppealMessages.Add(message);
        await context.SaveChangesAsync();

        return message;
    }
}
