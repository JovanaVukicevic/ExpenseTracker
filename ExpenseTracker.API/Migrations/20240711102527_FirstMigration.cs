using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpenseTrack.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BudgetCap = table.Column<double>(type: "float", nullable: false),
                    Indicator = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPremuium = table.Column<bool>(type: "bit", nullable: false),
                    SavingsAccountID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SavingsAccountID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SavingsAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetAmount = table.Column<double>(type: "float", nullable: false),
                    AmountPerMonth = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SavingsAccounts_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeIntervalInDays = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Indicator = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Schedules_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Indicator = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "Categories",
                        principalColumn: "Name");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0", null, "Admin", null },
                    { "1", null, "Premium", null },
                    { "2", null, "User", null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "userId1" },
                    { "2", "userId2" },
                    { "2", "userId3" },
                    { "2", "userId4" },
                    { "2", "userId5" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsPremuium", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SavingsAccountID", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "userId1", 0, "59b1ea18-926f-408a-ae22-8eb3e0199d97", "ivan.ivanovic123gmail.com", false, "Ivan", false, "Ivanovic", false, null, null, null, "AQAAAAIAAYagAAAAEEoGTHxrd0FxEaBMjHaTsRNAwo29xdbPpAxtvLuLS9zL/oexxftJl4Awys59gA9qvw==", null, false, 0, "bcbe6104-cc26-4e10-8cee-42eaab33b51a", false, "ivan1234" },
                    { "userId2", 0, "a3581ff2-7712-486c-b0ae-9541ce02cf3f", "jovan.ivanovic123gmail.com", false, "Jovan", false, "Ivanovic", false, null, null, null, "AQAAAAIAAYagAAAAEOccT5tgOBsdDZ62Jh1plawosxK/sOcK1XsRY6O8yaXKYgPwa+ePTtg3jtk2sEP53A==", null, false, 0, "b7dac079-371f-480a-88bb-8009756f2047", false, "jovan1234" },
                    { "userId3", 0, "24c91510-b3f3-4310-98d9-39e1b6acf6cd", "milica.bulat@gmail.com", false, "Milica", false, "Bulatovic", false, null, null, null, "AQAAAAIAAYagAAAAEOHS+ayVdh5bDTSq9x23PHlfLg67GTPbn6vAbT/9xUmnKDZ2T/J9TJxqx872WiHMfg==", null, false, 0, "0ea735d2-d2d9-4546-a67d-02bad549e332", false, "milica1234" },
                    { "userId4", 0, "38ce9013-a7aa-4a80-aeb1-9c1a8881d75a", "ivana.milos@gmail.com", false, "Ivana", false, "Milosevic", false, null, null, null, "AQAAAAIAAYagAAAAECgvKW+E7jHD+6mZDkyM6BjyMZ64xyHzJJ3lp7P/XlFgrI/FPr0hIe+YZf12YsIUZA==", null, false, 0, "70f8ae66-3ddb-4364-a434-cb86026c2e0b", false, "ivana123456" },
                    { "userId5", 0, "236d3101-9b0e-4855-9234-884633855006", "kaca.bulat@gmail.com", false, "Katarina", false, "Bulatovic", false, null, null, null, "AQAAAAIAAYagAAAAEFZ86p2x+t8aZkH4TzAI2tXyTCBseUM1K75kgoL/fnCpXkXCvbf6+zP+IsvhCY13YQ==", null, false, 0, "56e5fe84-7c0d-4164-beef-48ce4aa61cfd", false, "katarina1234" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_AccountID",
                table: "SavingsAccounts",
                column: "AccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_AccountID",
                table: "Schedules",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountID",
                table: "Transactions",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryName",
                table: "Transactions",
                column: "CategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SavingsAccounts");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
