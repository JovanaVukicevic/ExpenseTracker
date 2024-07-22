using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class ScheduledEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Schedules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37d90753-99de-443b-a093-b273c8d2d50d", "AQAAAAIAAYagAAAAEEb9wWptHIYJwiN6uuuH1KmF42eEAo+S2uO3QLaiXzscmvBQULKrqiUhSCQAQwQkPg==", "8c52ba71-1e30-4f3a-bba4-963b2e8bdcd9" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92556250-cb18-4a7c-8a1a-63bd4052a0c3", "AQAAAAIAAYagAAAAEMZkQh9IelGA1iKnoCv5od75FvFGJKRSMcFvz1ZRcD3Tashk2x7CT+PdUktyyG776A==", "e22fa06d-f8e5-4f5a-86dd-ad1d4bc1fff2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12a55ccc-a6c8-4b55-bf9c-479540b7a882", "AQAAAAIAAYagAAAAEE+d+SSRPj5hR2BKWWUEyMHKo6ZzmQALZ1F3Cb472IKqFP7KPRHwuv1zghJbsFmD1A==", "2015f3e6-466d-4e32-8993-666b9d51b56f" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "18a1cfb8-8169-4527-89e6-06c48b197bec", "AQAAAAIAAYagAAAAEJD/S5pznc68WD4pN3RaXfdmMIe0+H8W+JWB53SHtDBpBiGJRoFeVmIeP1xmHzrykA==", "e86c2a9c-0c47-42cf-8b89-8b902bd6189d" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3613c0a8-ae21-4e28-83ec-9c06894d15eb", "AQAAAAIAAYagAAAAEFdoQTB2yKuuaqk+hIecM0WB4kvGTDV35eGz6yAjwLgiGDrM8P4dWlwEe5QOJ/skWw==", "2ce92253-7b34-48cf-8eae-a07f0fe5bc36" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ae8974d-e187-46d4-974f-c2bc4fedf410", "AQAAAAIAAYagAAAAEEbhTlqUtBacLYRYyrTC2d3x4bD8Vbf11+pXjD1ArVEd8EKohwEcM2fUx8mbXrndWw==", "fd290807-bd90-4c1f-b22c-847f55f3f402" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0cc7a561-032f-4c14-ad3c-3c51d485e141", "AQAAAAIAAYagAAAAEFVaKw+TsZmjwJZre52QdpAjC5R7vzWw40oElnxWXqChwqYeRd3O3qm+Qo6LWiM59A==", "473f33c1-8daf-4c8d-b735-9d4969c81331" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8b4eaf0-f2bf-4248-9586-aa2444c315af", "AQAAAAIAAYagAAAAEE10xzI0W5ydXJoauuyH4K28K3Qd+hA63bjVVCccOM0OT32iYLAKJz64A+5LwItJuQ==", "d81f7dbf-0f4d-44d3-9777-f9e73d5bff6e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "582904b5-4f38-4147-8756-0664cef364ae", "AQAAAAIAAYagAAAAEHkLDG+wsJJiKhxcHhhlCv4Qhj1ppB/eSBZp3Hl4GILedV26C3fjgNEQ6n9GLF4LPQ==", "ef2f1522-a7db-470b-89c8-8dd0b6dc45af" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "userId5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd78cfcd-475c-4eee-8a83-192f43f00cd1", "AQAAAAIAAYagAAAAEO3dVWJEntMnZPuXm3NUvaWjQ3TYAmoresQ/pSE8SHVMpctgwkLmLgT5Fag0GetZWg==", "87c0dfc4-b4f2-49f9-a261-9498321be427" });
        }
    }
}
