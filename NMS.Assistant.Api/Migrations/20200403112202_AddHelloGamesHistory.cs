using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddHelloGamesHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelloGamesHistories",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false, defaultValue: 0),
                    Identifier = table.Column<string>(nullable: false),
                    DateDetected = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelloGamesHistories", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelloGamesHistories");
        }
    }
}
