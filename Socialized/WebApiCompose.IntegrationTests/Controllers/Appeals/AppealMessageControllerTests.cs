using Core.Providers.Hmac;
using Domain.Appeals;
using Domain.Users;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.UpdateAppealMessage;
using UseCases.Appeals.Replies.Commands.DeleteAppealMessageReply;

namespace WebApiCompose.IntegrationTests.Controllers.Appeals;

public class AppealMessageControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_ReturnOk()
    {
        // Arrange
        var appeal = await CreateTestAppeal();
        var command = new CreateAppealMessageCommand {
            Message = "Test message",
            UserToken = appeal.User.TokenForUse,
            AppealId = appeal.Id
        };
        var content = new MultipartFormDataContent();
        var jsonContent = JsonSerializer.Serialize(command);
        var commandContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        content.Add(commandContent, "commandJson");

        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Test file content"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "files", "message.json");
        content.Add(fileContent, "files", "message.json");

        // Act
        var response = await Client.PostAsync("/1.0/AppealMessage/Create", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var message = await context.AppealMessages.FirstOrDefaultAsync(t => t.AppealId == appeal.Id);
        message.Should().NotBeNull();
        message!.Message.Should().Be("Test message");
        var files = await context.AppealFiles.Where(t => t.MessageId == message.Id).ToListAsync();
        files.Should().HaveCount(2);
    }
    [Fact]
    public async Task Update_ReturnOk()
    {
        // Arrange
        var appeal = await CreateTestAppeal();
        var message = await CreateTestMessage(appeal);
        var command = new UpdateAppealMessageCommand
        {
            MessageId = message.Id,
            Message = "Test message updated"
        };
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync("/1.0/AppealMessage/Update/", content);
        var result = await response.Content.ReadFromJsonAsync<UpdateAppealMessageCommandResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Success.Should().BeTrue();
    }
    [Fact]
    public async Task Delete_ReturnOk()
    {
        // Arrange
        var appeal = await CreateTestAppeal();
        var message = await CreateTestMessage(appeal);
   
        // Act
        var response = await Client.DeleteAsync($"/1.0/AppealMessage/Delete?messageId={message.Id}");
        var result = await response.Content.ReadFromJsonAsync<DeleteAppealMessageReplyResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Success.Should().BeTrue();
    }
    public async Task<Appeal> CreateTestAppeal()
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
        };
        context.Appeals.Add(appeal);
        await context.SaveChangesAsync();

        return appeal;
    }
    public async Task<AppealMessage> CreateTestMessage(Appeal appeal)
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
