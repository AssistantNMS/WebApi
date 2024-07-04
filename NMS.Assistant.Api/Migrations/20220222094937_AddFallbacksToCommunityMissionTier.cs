using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddFallbacksToCommunityMissionTier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                table: "CommunityMissionRecordTiers",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FallbackImgUrl",
                table: "CommunityMissionRecordTiers",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FallbackTitle",
                table: "CommunityMissionRecordTiers",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FallbackImgUrl",
                table: "CommunityMissionRecordTiers");

            migrationBuilder.DropColumn(
                name: "FallbackTitle",
                table: "CommunityMissionRecordTiers");

            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                table: "CommunityMissionRecordTiers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);
        }
    }
}
