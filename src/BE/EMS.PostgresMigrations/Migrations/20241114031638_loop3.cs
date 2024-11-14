using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class loop3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagedDepartmentId",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagedDepartmentId",
                table: "Employees",
                column: "ManagedDepartmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_ManagedDepartmentId",
                table: "Employees",
                column: "ManagedDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_ManagedDepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ManagedDepartmentId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagedDepartmentId",
                table: "Employees");
        }
    }
}
