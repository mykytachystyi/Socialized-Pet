using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace WebApiCompose.IntegrationTests
{
    public class AuthenticationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new
            {
                Email = "test@example.com",
                Password = "ValidPassword123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<LoginResponseTest>();
            content.Should().NotBeNull();
            content.Token.Should().NotBeNullOrEmpty();
        }
    }

    public class LoginResponseTest
    {
        public string Token { get; set; }
    }
}