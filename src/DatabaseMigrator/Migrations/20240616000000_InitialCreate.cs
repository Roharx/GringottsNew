using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gringotts.Migrations.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                Username = table.Column<string>(nullable: false),
                Email = table.Column<string>(nullable: false),
                PasswordHash = table.Column<string>(nullable: false),
                DisplayName = table.Column<string>(nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                IsActive = table.Column<bool>(nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });
        migrationBuilder.CreateIndex("IX_Users_Username", "Users", "Username", unique: true);
        migrationBuilder.CreateIndex("IX_Users_Email", "Users", "Email", unique: true);
        migrationBuilder.CreateIndex("IX_Users_DisplayName", "Users", "DisplayName", unique: true);

        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                Name = table.Column<string>(nullable: false),
                Description = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ExchangeRates",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                FromCurrency = table.Column<string>(nullable: false),
                ToCurrency = table.Column<string>(nullable: false),
                Rate = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                EffectiveDate = table.Column<DateTime>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExchangeRates", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Balances",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                UserId = table.Column<Guid>(nullable: false),
                Currency = table.Column<string>(nullable: false),
                Amount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Balances", x => x.Id);
                table.ForeignKey(name: "FK_Balances_Users_UserId", column: x => x.UserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AuditLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                UserId = table.Column<Guid>(nullable: true),
                Action = table.Column<string>(nullable: true),
                Entity = table.Column<string>(nullable: true),
                EntityId = table.Column<Guid>(nullable: true),
                Timestamp = table.Column<DateTime>(nullable: true),
                Metadata = table.Column<string>(type: "jsonb", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuditLogs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BalanceSnapshots",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                UserId = table.Column<Guid>(nullable: false),
                Currency = table.Column<string>(nullable: true),
                Amount = table.Column<decimal>(nullable: true),
                RecordedAt = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BalanceSnapshots", x => x.Id);
                table.ForeignKey(name: "FK_BalanceSnapshots_Users_UserId", column: x => x.UserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RecurringTransactions",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                FromUserId = table.Column<Guid>(nullable: false),
                ToUserId = table.Column<Guid>(nullable: true),
                ExternalParty = table.Column<string>(nullable: true),
                Description = table.Column<string>(nullable: true),
                AmountKnuts = table.Column<int>(nullable: false),
                Currency = table.Column<string>(nullable: false),
                Frequency = table.Column<string>(nullable: false),
                NextOccurrence = table.Column<DateTime>(type: "date", nullable: false),
                IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecurringTransactions", x => x.Id);
                table.ForeignKey(name: "FK_RecurringTransactions_Users_FromUserId", column: x => x.FromUserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey(name: "FK_RecurringTransactions_Users_ToUserId", column: x => x.ToUserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                Timestamp = table.Column<DateTime>(nullable: true),
                Type = table.Column<string>(nullable: true),
                Galleons = table.Column<int>(nullable: false, defaultValue: 0),
                Sickles = table.Column<int>(nullable: false, defaultValue: 0),
                Knuts = table.Column<int>(nullable: false, defaultValue: 0),
                DkkAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                Description = table.Column<string>(nullable: true),
                FromUserId = table.Column<Guid>(nullable: true),
                ToUserId = table.Column<Guid>(nullable: true),
                ExternalParty = table.Column<string>(nullable: true),
                CategoryId = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.Id);
                table.ForeignKey(name: "FK_Transactions_Users_FromUserId", column: x => x.FromUserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey(name: "FK_Transactions_Users_ToUserId", column: x => x.ToUserId, principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey(name: "FK_Transactions_Categories_CategoryId", column: x => x.CategoryId, principalTable: "Categories", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.AddCheckConstraint("CK_Transactions_Type", "Transactions", "\"Type\" IN ('user-to-user','external-in','external-out','fee','exchange')");

        migrationBuilder.CreateIndex("IX_Balances_UserId", "Balances", "UserId");
        migrationBuilder.CreateIndex("IX_BalanceSnapshots_UserId", "BalanceSnapshots", "UserId");
        migrationBuilder.CreateIndex("IX_RecurringTransactions_FromUserId", "RecurringTransactions", "FromUserId");
        migrationBuilder.CreateIndex("IX_RecurringTransactions_ToUserId", "RecurringTransactions", "ToUserId");
        migrationBuilder.CreateIndex("IX_Transactions_FromUserId", "Transactions", "FromUserId");
        migrationBuilder.CreateIndex("IX_Transactions_ToUserId", "Transactions", "ToUserId");
        migrationBuilder.CreateIndex("IX_Transactions_CategoryId", "Transactions", "CategoryId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "BalanceSnapshots");
        migrationBuilder.DropTable(name: "Balances");
        migrationBuilder.DropTable(name: "RecurringTransactions");
        migrationBuilder.DropTable(name: "AuditLogs");
        migrationBuilder.DropTable(name: "Transactions");
        migrationBuilder.DropTable(name: "ExchangeRates");
        migrationBuilder.DropTable(name: "Categories");
        migrationBuilder.DropTable(name: "Users");
    }
}
