using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddCommunityLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityLinks",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Subtitle = table.Column<string>(nullable: false),
                    ExternalUrl = table.Column<string>(nullable: false),
                    IconUrl = table.Column<string>(nullable: false),
                    SortRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityLinks", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 19);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityLinks");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 20);
        }
    }
}
