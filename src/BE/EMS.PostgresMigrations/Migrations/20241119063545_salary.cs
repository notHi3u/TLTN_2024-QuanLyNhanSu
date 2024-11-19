using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class salary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deductions",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FlatBonus",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PercentBonus",
                table: "Employees",
                newName: "Salary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Employees",
                newName: "PercentBonus");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "Employees",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Deductions",
                table: "Employees",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FlatBonus",
                table: "Employees",
                type: "numeric",
                nullable: true);
        }
    }
}
