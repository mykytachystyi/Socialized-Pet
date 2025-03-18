using WebApiCompose.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Infrastructure;

namespace WebApiCompose.IntegrationTests;

public class IntegrationTestBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private TestWebApplicationFactory? _application;
    private HttpClient? _client;

    protected HttpClient Client => _client ?? throw new InvalidOperationException("Client is not initialized");
    protected TestWebApplicationFactory Application => _application ?? throw new InvalidOperationException("Application is not initialized");

    protected IntegrationTestBase()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("test_db")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _application = new TestWebApplicationFactory(_dbContainer.GetConnectionString());
        _client = _application.CreateClient();

        using var scope = _application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        if (_client != null)
        {
            _client.Dispose();
        }

        if (_application != null)
        {
            await _application.DisposeAsync();
        }

        await _dbContainer.DisposeAsync();
    }
}