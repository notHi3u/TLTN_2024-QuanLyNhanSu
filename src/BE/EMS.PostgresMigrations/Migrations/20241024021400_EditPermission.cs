using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class EditPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "EMS",
                table: "Permissions");

            migrationBuilder.InsertData(
                schema: "EMS",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "EmployeeId", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Administrator Role", null, "Admin", null },
                    { "2", null, "Employee Role", null, "Employee", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "EMS",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                schema: "EMS",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                schema: "EMS",
                table: "Permissions",
                type: "text",
                nullable: true);
        }
    }
}
