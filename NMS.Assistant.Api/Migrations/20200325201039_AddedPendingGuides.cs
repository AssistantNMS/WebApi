using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddedPendingGuides : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GuideMetaDatas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PendingGuides",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    GuideMetaGuid = table.Column<Guid>(nullable: false),
                    UserContactDetails = table.Column<string>(nullable: false),
                    FileRelativePath = table.Column<string>(nullable: false),
                    DateSubmitted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingGuides", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_PendingGuides_GuideMetas",
                        column: x => x.GuideMetaGuid,
                        principalTable: "GuideMetaDatas",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingGuides_GuideMetaGuid",
                table: "PendingGuides",
                column: "GuideMetaGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingGuides");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GuideMetaDatas");
        }
    }
}
