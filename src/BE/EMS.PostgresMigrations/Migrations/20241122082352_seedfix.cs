using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class seedfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "16bbc0bb-12fe-4ecc-8bc6-4e9a6b4563a9", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAELZ9eD+5IKOgemH1pduVkjjhVkC37cAF5VpoxVLqhFfrjetMi3ifLUwHPfu5w6q/Nw==", "abb94131-b1f4-45be-bfa1-cb28b6290752", "admin@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "0cea5e41-515c-4943-973c-869d37d5f4b2", "adminUser@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEM2PiwuXWvbjMnd5TuRr8PTQLyk36IaNCVmGy1EZtbusixmpZWkh8ZW4QHC9TqVFtQ==", "fe184064-84eb-405d-93b9-aac29915b86b", "admin" });
        }
    }
}
