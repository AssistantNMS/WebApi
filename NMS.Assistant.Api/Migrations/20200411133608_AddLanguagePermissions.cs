using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddLanguagePermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageSubmissions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Filename = table.Column<string>(nullable: false),
                    UserContactDetails = table.Column<string>(nullable: false),
                    DateSubmitted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSubmissions", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 24);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 25);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageSubmissions");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 25);
        }
    }
}
