using Microsoft.EntityFrameworkCore;
using Template.Infrastructure.Data.Contexts;
using Template.Infrastructure.Data.Seed;

namespace Template.API.Extensions
{
    public static class DatabaseSeederExtensions
    {
        /// <summary>
        /// Migrates both IdentityDbContext and AppDbContext databases
        /// </summary>
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                // Migrate IdentityDbContext
                var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();
                logger.LogInformation("Identity database migration completed successfully");

                // Migrate AppDbContext
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await appDbContext.Database.MigrateAsync();
                logger.LogInformation("App database migration completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the databases");
                throw;
            }

            return app;
        }

        /// <summary>
        /// Seeds the database with default data (users, roles, etc.)
        /// </summary>
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
                await seeder.SeedAsync();
                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }

            return app;
        }
    }
}
