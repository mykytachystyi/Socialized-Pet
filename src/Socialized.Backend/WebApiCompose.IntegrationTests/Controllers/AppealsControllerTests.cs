using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UseCases.Appeals.Commands.CreateAppeal;
using UseCases.Appeals.Models;

namespace WebApiCompose.IntegrationTests.Controllers;

public class AppealsControllerTests : IntegrationTestContext
{
    [Fact]
    public async Task Create_ReturnOk()
    {
        // Arrange
        await CreateTestUser();
        await SetupUser(ActualUser);
        var content = new StringContent(JsonSerializer.Serialize(
            new CreateAppealCommand
            {
                Subject = "Test subject",
            }),
            Encoding.UTF8,
            "application/json"
        );
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualUserJwtToken);

        // Act
        var response = await Client.PostAsync("/1.0/Appeals/Create/", content);
        var result = await response.Content.ReadFromJsonAsync<AppealResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify database
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var appeal = await context.Appeals.FirstOrDefaultAsync(t => t.UserId == ActualUser.Id);
        appeal.Should().NotBeNull();
        appeal!.Subject.Should().Be("Test subject");
    }
    [Fact]
    public async Task GetAppealsByUser_ReturnCollectionAppeals()
    {
        // Arrange
        await CreateTestUser();
        await SetupUser(ActualUser);
        var appeal = CreateAppeal(ActualUser);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualUserJwtToken);

        // Act
        var response = await Client.GetAsync($"/1.0/Appeals/GetAppealsByUser?&since=0&count=10");
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppealResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Count().Should().Be(1);
    }
    [Fact]
    public async Task GetAppealsByAdmin_ReturnCollectionAppeals()
    {
        // Arrange
        var user = await CreateTestUser();
        var admin = await CreateTestAdmin();
        var token = await SetupAdmin(admin);
        var appeal = CreateAppeal(user);

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.GetAsync($"/1.0/Appeals/GetAppealsByAdmin?since=0&count=10");
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppealResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Count().Should().Be(1);
    }
}