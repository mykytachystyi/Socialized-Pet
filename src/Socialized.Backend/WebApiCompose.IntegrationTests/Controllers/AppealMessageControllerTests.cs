using Domain.Appeals;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Appeals.Messages.CreateAppealMessage;
using UseCases.Appeals.Messages.DeleteAppealMessage;
using UseCases.Appeals.Messages.UpdateAppealMessage;

namespace WebApiCompose.IntegrationTests.Controllers;

public class AppealMessageControllerTests : AppealsControllerTests
{
    [Fact]
    public async Task CreateAppeal_ReturnOk()
    {
        // Arrange
        var user = await CreateTestUser();
        var token = await SetupUser(user);
        var appeal = await CreateAppeal(user);
        var command = new CreateAppealMessageCommand
        {
            Message = "Test message",
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

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
    public async Task UpdateAppeal_ReturnOk()
    {
        // Arrange
        var user = await CreateTestUser();
        var token = await SetupUser(user);
        var appeal = await CreateAppeal(user);
        var appealMessage = await CreateMessage(appeal);

        var command = new UpdateAppealMessageCommand
        {
            MessageId = appealMessage.Id,
            Message = "Test message updated"
        };
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await Client.PutAsync("/1.0/AppealMessage/Update/", content);
        var result = await response.Content.ReadFromJsonAsync<UpdateAppealMessageCommandResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Success.Should().BeTrue();
    }
    [Fact]
    public async Task DeleteAppeal_ReturnOk()
    {
        // Arrange
        var user = await CreateTestUser();
        var token = await SetupUser(user);
        var appeal = await CreateAppeal(user);
        var appealMessage = await CreateMessage(appeal);


        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await Client.DeleteAsync($"/1.0/AppealMessage/Delete?messageId={appealMessage.Id}");
        var result = await response.Content.ReadFromJsonAsync<DeleteAppealMessageResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Success.Should().BeTrue();
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
