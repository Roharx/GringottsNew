using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Gringotts.Migrations;

namespace Gringotts.Migrations.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity("Balance", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<string>("Currency");
            b.Property<decimal>("Amount").HasColumnType("numeric(18,4)");
            b.Property<Guid>("UserId");
            b.Property<DateTime>("UpdatedAt").HasDefaultValueSql("now()");
            b.HasKey("Id");
            b.HasIndex("UserId");
        });

        modelBuilder.Entity("BalanceSnapshot", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<decimal?>("Amount");
            b.Property<string?>("Currency");
            b.Property<DateTime?>("RecordedAt");
            b.Property<Guid>("UserId");
            b.HasKey("Id");
            b.HasIndex("UserId");
        });

        modelBuilder.Entity("AuditLog", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<string?>("Action");
            b.Property<string?>("Entity");
            b.Property<Guid?>("EntityId");
            b.Property<string?>("Metadata").HasColumnType("jsonb");
            b.Property<DateTime?>("Timestamp");
            b.Property<Guid?>("UserId");
            b.HasKey("Id");
        });

        modelBuilder.Entity("Category", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<string>("Name");
            b.Property<string?>("Description");
            b.HasKey("Id");
        });

        modelBuilder.Entity("ExchangeRate", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<string>("FromCurrency");
            b.Property<string>("ToCurrency");
            b.Property<decimal>("Rate").HasColumnType("numeric(18,8)");
            b.Property<DateTime>("EffectiveDate").HasColumnType("date");
            b.HasKey("Id");
        });

        modelBuilder.Entity("RecurringTransaction", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<int>("AmountKnuts");
            b.Property<string>("Currency");
            b.Property<string?>("Description");
            b.Property<string?>("ExternalParty");
            b.Property<Guid>("FromUserId");
            b.Property<bool>("IsActive").HasDefaultValue(true);
            b.Property<DateTime>("CreatedAt").HasDefaultValueSql("now()");
            b.Property<DateTime>("NextOccurrence").HasColumnType("date");
            b.Property<string>("Frequency");
            b.Property<Guid?>("ToUserId");
            b.HasKey("Id");
            b.HasIndex("FromUserId");
            b.HasIndex("ToUserId");
        });

        modelBuilder.Entity("Transaction", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<Guid?>("CategoryId");
            b.Property<decimal?>("DkkAmount").HasColumnType("numeric(10,2)");
            b.Property<string?>("Description");
            b.Property<int>("Galleons").HasDefaultValue(0);
            b.Property<int>("Knuts").HasDefaultValue(0);
            b.Property<Guid?>("FromUserId");
            b.Property<string?>("ExternalParty");
            b.Property<int>("Sickles").HasDefaultValue(0);
            b.Property<DateTime?>("Timestamp");
            b.Property<Guid?>("ToUserId");
            b.Property<string?>("Type");
            b.HasKey("Id");
            b.HasIndex("CategoryId");
            b.HasIndex("FromUserId");
            b.HasIndex("ToUserId");
        });

        modelBuilder.Entity("User", b =>
        {
            b.Property<Guid>("Id").HasDefaultValueSql("uuid_generate_v4()");
            b.Property<DateTime>("CreatedAt").HasDefaultValueSql("now()");
            b.Property<string>("DisplayName");
            b.Property<string>("Email");
            b.Property<bool>("IsActive").HasDefaultValue(true);
            b.Property<string>("PasswordHash");
            b.Property<string>("Username");
            b.HasKey("Id");
            b.HasIndex("DisplayName").IsUnique();
            b.HasIndex("Email").IsUnique();
            b.HasIndex("Username").IsUnique();
        });
    }
}
