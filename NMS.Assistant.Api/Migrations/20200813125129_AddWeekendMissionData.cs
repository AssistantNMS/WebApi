using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddWeekendMissionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeekendMissions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    SeasonId = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    IsConfirmedByCaptSteve = table.Column<bool>(nullable: false),
                    IsConfirmedByAssistantNms = table.Column<bool>(nullable: false),
                    ActiveDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekendMissions", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 30);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 31);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekendMissions");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 31);
        }
    }
}
