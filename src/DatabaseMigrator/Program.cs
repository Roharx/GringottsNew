using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Gringotts.Migrations;
using System;

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

// Apply migrations (will create DB if it does not exist)
db.Database.Migrate();
