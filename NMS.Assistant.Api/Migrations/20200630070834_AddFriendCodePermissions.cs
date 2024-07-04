using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddFriendCodePermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendCodes",
                table: "FriendCodes");

            migrationBuilder.RenameTable(
                name: "FriendCodes",
                newName: "FriendCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendCode",
                table: "FriendCode",
                column: "Guid");

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 28);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 29);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendCode",
                table: "FriendCode");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 29);

            migrationBuilder.RenameTable(
                name: "FriendCode",
                newName: "FriendCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendCodes",
                table: "FriendCodes",
                column: "Guid");
        }
    }
}
