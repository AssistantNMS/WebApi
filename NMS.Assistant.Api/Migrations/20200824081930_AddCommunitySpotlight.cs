using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddCommunitySpotlight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunitySpotlights",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    UserImage = table.Column<string>(maxLength: 100, nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(maxLength: 2000, nullable: false),
                    ExternalUrl = table.Column<string>(maxLength: 200, nullable: false),
                    PreviewImageUrl = table.Column<string>(maxLength: 200, nullable: false),
                    SortRank = table.Column<int>(nullable: false),
                    ActiveDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunitySpotlights", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 32);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 33);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunitySpotlights");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 33);
        }
    }
}
