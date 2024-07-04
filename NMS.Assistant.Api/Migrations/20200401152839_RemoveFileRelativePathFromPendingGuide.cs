using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class RemoveFileRelativePathFromPendingGuide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileRelativePath",
                table: "PendingGuides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileRelativePath",
                table: "PendingGuides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
