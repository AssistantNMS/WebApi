using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddFriendCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendCodes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PlatformType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    PcVisible = table.Column<bool>(nullable: false),
                    Ps4Visible = table.Column<bool>(nullable: false),
                    Xb1Visible = table.Column<bool>(nullable: false),
                    SortRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendCodes", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendCodes");
        }
    }
}
