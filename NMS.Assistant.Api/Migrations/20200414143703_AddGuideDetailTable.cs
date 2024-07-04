using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddGuideDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuideDetails",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    DetailGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    ShortTitle = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    Minutes = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Tags = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideDetails", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_GuideDetails_GuideMetas",
                        column: x => x.Guid,
                        principalTable: "GuideMetaDatas",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuideDetails");
        }
    }
}
