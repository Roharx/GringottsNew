using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Gringotts.Migrations;
using System;
using System.Threading.Tasks;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var connection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") ??
            "Host=postgres;Username=gringotts;Password=secret;Database=gringotts";
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
    })
    .Build();

using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

await WaitForDatabaseAsync(scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migrator"));

// Apply migrations (will create DB if it does not exist)
db.Database.Migrate();

static async Task WaitForDatabaseAsync(ILogger logger)
{
    var connString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") ??
        "Host=postgres;Username=gringotts;Password=secret;Database=gringotts";
    const int maxRetries = 5;
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();
            logger.LogInformation("Database connection succeeded");
            return;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database not ready, attempt {Attempt} of {Max}", attempt, maxRetries);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
    throw new Exception("Unable to connect to database");
}
