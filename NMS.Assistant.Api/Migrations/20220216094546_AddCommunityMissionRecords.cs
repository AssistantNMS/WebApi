using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddCommunityMissionRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityMissionRecords",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    MissionId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMissionRecords", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "CommunityMissionRecordTiers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    CommunityMissionGuid = table.Column<Guid>(nullable: false),
                    Tier = table.Column<int>(nullable: false),
                    AppId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMissionRecordTiers", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_Tiers_CommunityMission",
                        column: x => x.CommunityMissionGuid,
                        principalTable: "CommunityMissionRecords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 36);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 37);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMissionRecordTiers_CommunityMissionGuid",
                table: "CommunityMissionRecordTiers",
                column: "CommunityMissionGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMissionRecordTiers");

            migrationBuilder.DropTable(
                name: "CommunityMissionRecords");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 37);
        }
    }
}
