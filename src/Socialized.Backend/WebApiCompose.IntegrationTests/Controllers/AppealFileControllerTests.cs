using Domain.Appeals;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApiCompose.IntegrationTests.Controllers;

public class AppealFileControllerTests : AppealMessageControllerTests
{
    [Fact]
    public async Task CreateAppealFile_ReturnOk()
    {
        // Arrange
        var user = await CreateTestUser();
        var token = await SetupUser(user);
        var appeal = await CreateAppeal(user);
        var appealMessage = await CreateMessage(appeal);

        var content = new MultipartFormDataContent();
        var jsonContent = JsonSerializer.Serialize(new AppealMessage { Message = "Test message" });
        var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(jsonContent));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "files", "message.json");
        content.Add(fileContent, "files", "message.json");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await Client.PostAsync($"/1.0/AppealFile/Create?messageId={appealMessage.Id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database 
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var file = await context.AppealFiles.FirstOrDefaultAsync(t => 
            t.MessageId == appealMessage.Id);
        file.Should().NotBeNull();
        file!.MessageId.Should().Be(appealMessage.Id);
        file!.RelativePath.Should().NotBeNullOrEmpty();
    }
}