using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class AppDbInitializer
    {
        public static async Task ApplyMigrationsAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<AppDbContext>>();
            try
            {
                var context = services.GetRequiredService<AppDbContext>();

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Були застосование міграції бази даних.");
                }
                else
                {
                    logger.LogInformation("Міграції баз даних не були застосовані.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Виникла помилка при ініціалізаціх бази даних.");
                throw;
            }
        }
    }
}