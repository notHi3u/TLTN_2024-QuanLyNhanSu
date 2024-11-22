using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class syncSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlatBonus",
                table: "SalaryRecords");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                table: "SalaryRecords");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PercentBonus",
                table: "SalaryRecords",
                newName: "Bonuses");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "Employees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Bonuses",
                table: "Employees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Deductions",
                table: "Employees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Bonuses",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deductions",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Bonuses",
                table: "SalaryRecords",
                newName: "PercentBonus");

            migrationBuilder.AddColumn<decimal>(
                name: "FlatBonus",
                table: "SalaryRecords",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetSalary",
                table: "SalaryRecords",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Employees",
                type: "numeric",
                nullable: true);
        }
    }
}
