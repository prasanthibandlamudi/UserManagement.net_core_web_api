using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class tablesnamesh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Mst_Users");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Mst_Roles");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "Mst_Users",
                newName: "IX_Mst_Users_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mst_Users",
                table: "Mst_Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mst_Roles",
                table: "Mst_Roles",
                column: "RoleId");

            migrationBuilder.UpdateData(
                table: "Mst_Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 29, 11, 33, 54, 632, DateTimeKind.Utc).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "Mst_Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 29, 11, 33, 54, 632, DateTimeKind.Utc).AddTicks(7753));

            migrationBuilder.AddForeignKey(
                name: "FK_Mst_Users_Mst_Roles_RoleId",
                table: "Mst_Users",
                column: "RoleId",
                principalTable: "Mst_Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mst_Users_Mst_Roles_RoleId",
                table: "Mst_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mst_Users",
                table: "Mst_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mst_Roles",
                table: "Mst_Roles");

            migrationBuilder.RenameTable(
                name: "Mst_Users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Mst_Roles",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_Mst_Users_RoleId",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 10, 51, 19, 190, DateTimeKind.Utc).AddTicks(553));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 10, 51, 19, 190, DateTimeKind.Utc).AddTicks(730));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
