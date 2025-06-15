using System;
using System.Threading.Tasks;
using Npgsql;
using Xunit;

public class DatabaseSetupTests
{
    [Fact]
    public async Task UsersTableExists()
    {
        var connString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") ??
            "Host=postgres;Username=gringotts;Password=secret;Database=gringotts";
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM information_schema.tables WHERE table_name = 'Users'", conn);
        var count = (long) (await cmd.ExecuteScalarAsync() ?? 0);
        Assert.True(count > 0, "Users table should exist");
    }
}
