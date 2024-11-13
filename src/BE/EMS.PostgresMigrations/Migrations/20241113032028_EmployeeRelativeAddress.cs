using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeRelativeAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "EmployeeRelatives",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "EmployeeRelatives",
                newName: "Adress");
        }
    }
}
