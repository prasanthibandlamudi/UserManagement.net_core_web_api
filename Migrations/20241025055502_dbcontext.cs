using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class dbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "ActiveStatus", "CreatedBy", "CreatedDate", "RoleName" },
                values: new object[] { 1, true, "System", new DateTime(2024, 10, 25, 5, 55, 2, 235, DateTimeKind.Utc).AddTicks(7014), "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "ActiveStatus", "CreatedBy", "CreatedDate", "Email", "Password", "PhoneNumber", "RoleId", "UserName" },
                values: new object[] { 1, true, "System", new DateTime(2024, 10, 25, 5, 55, 2, 235, DateTimeKind.Utc).AddTicks(7142), "superadmin@gmail.com", "superadmin@123", null, 1, "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
