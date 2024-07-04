using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddPendingFriendCodeTableBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendCode",
                table: "FriendCode");

            migrationBuilder.RenameTable(
                name: "FriendCode",
                newName: "FriendCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendCodes",
                table: "FriendCodes",
                column: "Guid");

            migrationBuilder.CreateTable(
                name: "PendingFriendCodes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    EmailHash = table.Column<string>(nullable: false),
                    PlatformType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    DateSubmitted = table.Column<DateTime>(nullable: false),
                    SortRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingFriendCodes", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingFriendCodes");

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
        }
    }
}
