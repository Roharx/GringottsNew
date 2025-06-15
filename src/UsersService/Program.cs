using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading.Tasks;

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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

await WaitForDatabaseAsync(app.Logger);

app.MapGet("/", () => "Hello from UsersService!");

app.Run();
