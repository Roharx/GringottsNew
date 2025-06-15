using System;
using Microsoft.EntityFrameworkCore;

namespace Gringotts.Migrations;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();
    public DbSet<Balance> Balances => Set<Balance>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<BalanceSnapshot> BalanceSnapshots => Set<BalanceSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.HasIndex(x => x.Username).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.DisplayName).IsUnique();
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<ExchangeRate>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
            e.HasOne<User>().WithMany().HasForeignKey(x => x.FromUserId);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.ToUserId);
            e.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId);
            e.HasCheckConstraint("CK_Transactions_Type", "\"Type\" IN ('user-to-user','external-in','external-out','fee','exchange')");
        });

        modelBuilder.Entity<RecurringTransaction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            e.HasOne<User>().WithMany().HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.ToUserId);
        });

        modelBuilder.Entity<Balance>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<BalanceSnapshot>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
        });
    }
}

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class ExchangeRate
{
    public Guid Id { get; set; }
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class Transaction
{
    public Guid Id { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Type { get; set; }
    public int Galleons { get; set; }
    public int Sickles { get; set; }
    public int Knuts { get; set; }
    public decimal? DkkAmount { get; set; }
    public string? Description { get; set; }
    public Guid? FromUserId { get; set; }
    public Guid? ToUserId { get; set; }
    public string? ExternalParty { get; set; }
    public Guid? CategoryId { get; set; }
}

public class RecurringTransaction
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid? ToUserId { get; set; }
    public string? ExternalParty { get; set; }
    public string? Description { get; set; }
    public int AmountKnuts { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateTime NextOccurrence { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class Balance
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? Action { get; set; }
    public string? Entity { get; set; }
    public Guid? EntityId { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Metadata { get; set; }
}

public class BalanceSnapshot
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Currency { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? RecordedAt { get; set; }
}
