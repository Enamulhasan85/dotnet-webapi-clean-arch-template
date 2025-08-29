using Microsoft.EntityFrameworkCore;
using Template.API.Extensions;
using Template.Application;
using Template.Infrastructure;
using Template.Infrastructure.Data.Contexts;

namespace Template.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebApiServices(builder.Configuration);

            var app = builder.Build();

            // Migrate databases
            await app.MigrateDatabaseAsync();

            // Seed database with default data
            await app.SeedDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseResponseCompression();

            app.UseExceptionHandler("/error");

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
