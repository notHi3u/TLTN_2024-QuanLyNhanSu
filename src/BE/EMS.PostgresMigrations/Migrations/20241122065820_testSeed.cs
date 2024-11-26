using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class testSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000000", null, "ADMIN", "Admin", null },
                    { "00000000-0000-0000-0000-000000000001", null, "HR", "HR", null },
                    { "00000000-0000-0000-0000-000000000002", null, "DEPARTMENT MANAGER", "Department Manager", null },
                    { "00000000-0000-0000-0000-000000000003", null, "EMPLOYEE", "Employee", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000100", 0, "5a103b79-2cb1-436a-ab4a-1efc192706c2", "admin@example.com", true, false, null, null, null, null, null, false, "5bb1c38e-aa11-4790-8fc8-bcbecea613fc", false, "admin" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name", "RoleId", "UserId" },
                values: new object[,]
                {
                    { "perm_approve_timecards", "Ability to approve employee timecards", "Approve Timecards", null, null },
                    { "perm_manage_departments", "Ability to manage department records", "Manage Departments", null, null },
                    { "perm_manage_employees", "Ability to manage employee records", "Manage Employees", null, null },
                    { "perm_manage_permissions", "Ability to manage user permissions", "Manage Permissions", null, null },
                    { "perm_manage_personal_info", "Ability to manage personal information", "Manage Personal Info", null, null },
                    { "perm_manage_positions", "Ability to manage job positions", "Manage Job Positions", null, null },
                    { "perm_manage_salary", "Ability to manage employee salaries", "Manage Salary", null, null },
                    { "perm_view_reports", "Ability to view salary and work reports", "View Reports", null, null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { "perm_approve_timecards", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_departments", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_employees", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_permissions", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_positions", "00000000-0000-0000-0000-000000000000" },
                    { "perm_manage_salary", "00000000-0000-0000-0000-000000000000" },
                    { "perm_view_reports", "00000000-0000-0000-0000-000000000000" },
                    { "perm_approve_timecards", "00000000-0000-0000-0000-000000000001" },
                    { "perm_manage_departments", "00000000-0000-0000-0000-000000000001" },
                    { "perm_manage_employees", "00000000-0000-0000-0000-000000000001" },
                    { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000001" },
                    { "perm_manage_positions", "00000000-0000-0000-0000-000000000001" },
                    { "perm_manage_salary", "00000000-0000-0000-0000-000000000001" },
                    { "perm_view_reports", "00000000-0000-0000-0000-000000000001" },
                    { "perm_approve_timecards", "00000000-0000-0000-0000-000000000002" },
                    { "perm_manage_departments", "00000000-0000-0000-0000-000000000002" },
                    { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000002" },
                    { "perm_view_reports", "00000000-0000-0000-0000-000000000002" },
                    { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000003" },
                    { "perm_view_reports", "00000000-0000-0000-0000-000000000003" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000100");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_approve_timecards", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_departments", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_employees", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_permissions", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_positions", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_salary", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_view_reports", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_approve_timecards", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_departments", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_employees", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_positions", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_salary", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_view_reports", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_approve_timecards", "00000000-0000-0000-0000-000000000002" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_departments", "00000000-0000-0000-0000-000000000002" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000002" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_view_reports", "00000000-0000-0000-0000-000000000002" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_manage_personal_info", "00000000-0000-0000-0000-000000000003" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "perm_view_reports", "00000000-0000-0000-0000-000000000003" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000002");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000003");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_approve_timecards");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_departments");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_employees");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_permissions");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_personal_info");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_positions");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_manage_salary");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "perm_view_reports");
        }
    }
}
