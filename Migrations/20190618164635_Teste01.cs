using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreCore.Migrations
{
    public partial class Teste01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_ApplicationRoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_ApplicationUserId",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "UserRoles",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "ApplicationRoleId",
                table: "UserRoles",
                newName: "RoleId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_ApplicationUserId",
                table: "UserRoles",
                newName: "IX_UserRoles_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_ApplicationRoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                table: "UserRoles",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId1",
                table: "UserRoles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId1",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "UserRoles",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "RoleId1",
                table: "UserRoles",
                newName: "ApplicationRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserId1",
                table: "UserRoles",
                newName: "IX_UserRoles_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId1",
                table: "UserRoles",
                newName: "IX_UserRoles_ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_ApplicationRoleId",
                table: "UserRoles",
                column: "ApplicationRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_ApplicationUserId",
                table: "UserRoles",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
