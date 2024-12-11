using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class img : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Employees",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "79872cfa-04b4-45ff-b6ce-ed836621d756", "AQAAAAIAAYagAAAAEOkPmmoKX/41kQ1KEfOeyi2W7kuzkamk8h+C1WPMHkflOfAdaVezwTa4DONw8oujvA==", "a0d7fba0-008a-42ee-9f5d-8effd442bd31" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "16bbc0bb-12fe-4ecc-8bc6-4e9a6b4563a9", "AQAAAAIAAYagAAAAELZ9eD+5IKOgemH1pduVkjjhVkC37cAF5VpoxVLqhFfrjetMi3ifLUwHPfu5w6q/Nw==", "abb94131-b1f4-45be-bfa1-cb28b6290752" });
        }
    }
}
