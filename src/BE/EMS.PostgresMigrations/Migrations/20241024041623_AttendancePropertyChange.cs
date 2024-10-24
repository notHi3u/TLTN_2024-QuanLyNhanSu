using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AttendancePropertyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Employees_EmpployeeId",
                schema: "EMS",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "EmpployeeId",
                schema: "EMS",
                table: "Attendances",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EmpployeeId",
                schema: "EMS",
                table: "Attendances",
                newName: "IX_Attendances_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                schema: "EMS",
                table: "Attendances",
                column: "EmployeeId",
                principalSchema: "EMS",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                schema: "EMS",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "EMS",
                table: "Attendances",
                newName: "EmpployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EmployeeId",
                schema: "EMS",
                table: "Attendances",
                newName: "IX_Attendances_EmpployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmpployeeId",
                schema: "EMS",
                table: "Attendances",
                column: "EmpployeeId",
                principalSchema: "EMS",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
