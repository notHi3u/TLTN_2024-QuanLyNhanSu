using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class testSeed4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0cea5e41-515c-4943-973c-869d37d5f4b2", "AQAAAAIAAYagAAAAEM2PiwuXWvbjMnd5TuRr8PTQLyk36IaNCVmGy1EZtbusixmpZWkh8ZW4QHC9TqVFtQ==", "fe184064-84eb-405d-93b9-aac29915b86b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d7406c2-5b3f-4e03-be91-7f127017ea54", "AQAAAAIAAYagAAAAEDtMzbymyy53IJM7W5KdICjMFIZzf/WmfKwsDWAQyJuIQzgaDwxOyJyyBrTCvKuG8A==", "037b955e-1015-47cb-9ec5-c9d324d655ca" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", null, "Admin", "ADMIN" });
        }
    }
}
