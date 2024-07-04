using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class CommunityMissionProgressTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityMissionsProgress",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    CommunityMissionGuid = table.Column<Guid>(nullable: false),
                    Tier = table.Column<int>(nullable: false),
                    Percentage = table.Column<int>(nullable: false),
                    DateRecorded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMissionsProgress", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_Progress_CommunityMission",
                        column: x => x.CommunityMissionGuid,
                        principalTable: "CommunityMissionRecords",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMissionsProgress_CommunityMissionGuid",
                table: "CommunityMissionsProgress",
                column: "CommunityMissionGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMissionsProgress");
        }
    }
}
