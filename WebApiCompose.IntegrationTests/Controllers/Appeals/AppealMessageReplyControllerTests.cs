using Core.Providers.Hmac;
using Domain.Users;
using Infrastructure;
using Domain.Appeals;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Admins.Commands.Authentication;
using WebApiCompose.Responses;
using UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;
using UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;

namespace WebApiCompose.IntegrationTests.Controllers.Appeals;

public class AppealMessageReplyControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_ReturnOk()
    {
        // Arrange
        var message = await CreateTestAppealMessage();
        var adminToken = await GetAdminToken();
        var content = new StringContent(JsonSerializer.Serialize(
            new CreateAppealMessageReplyCommand
            {
                AppealMessageId = message.Id,
                Reply = "Test reply message",
            }),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var response = await Client.PostAsync("/1.0/AppealMessageReply/Create/", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var reply = await context.AppealReplies.FirstOrDefaultAsync(t => t.AppealMessageId == message.Id);
        reply.Should().NotBeNull();
        reply!.Reply.Should().Be("Test reply message");
    }
    [Fact]
    public async Task Update_ReturnOk()
    {
        // Arrange
        var message = await CreateTestAppealMessage();
        var reply = await CreateTestReply(message);
        var adminToken = await GetAdminToken();
        var content = new StringContent(JsonSerializer.Serialize(
            new UpdateAppealMessageReplyCommand
            {
                ReplyId = reply.Id,
                Reply = "Updated test reply message"
            }),
            Encoding.UTF8, "application/json"
        );

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var response = await Client.PutAsync("/1.0/AppealMessageReply/Update/", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var replyResult = await context.AppealReplies.FirstOrDefaultAsync(t => t.AppealMessageId == message.Id);
        replyResult.Should().NotBeNull();
        replyResult!.Reply.Should().Be("Updated test reply message");
    }
    [Fact]
    public async Task Delete_ReturnOk()
    {
        // Arrange
        var message = await CreateTestAppealMessage();
        var reply = await CreateTestReply(message);
        var adminToken = await GetAdminToken();

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var response = await Client.DeleteAsync($"/1.0/AppealMessageReply/Delete?replyId={reply.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var replyResult = await context.AppealReplies.FirstOrDefaultAsync(t => t.AppealMessageId == message.Id);
        replyResult.Should().NotBeNull();
        replyResult!.IsDeleted.Should().BeTrue();
    }
    public async Task<string> GetAdminToken()
    {
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

        return resultAdmin.AdminToken;
    }
    public async Task<AppealMessage> CreateTestAppealMessage()
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
    public async Task<AppealMessageReply> CreateTestReply(AppealMessage message)
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var reply = new AppealMessageReply
        {
            AppealMessageId = message.Id,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = DateTimeOffset.UtcNow,
            Reply = "Test reply",
            IsDeleted = false,
            LastUpdatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
        context.AppealReplies.Add(reply);
        await context.SaveChangesAsync();
        return reply;
    }
}