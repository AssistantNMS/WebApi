using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddGuideDetailsBackWithManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuideDetails",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "GuideMetaGuideDetails",
                columns: table => new
                {
                    GuideMetaGuid = table.Column<Guid>(nullable: false),
                    GuideDetailGuid = table.Column<Guid>(nullable: false),
                    LanguageType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideMetaGuideDetails", x => new { x.GuideMetaGuid, x.GuideDetailGuid, x.LanguageType });
                    table.ForeignKey(
                        name: "ForeignKey_GuideMetaGuideDetails_GuideDetails",
                        column: x => x.GuideDetailGuid,
                        principalTable: "GuideDetails",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_GuideMetaGuideDetails_GuideMetas",
                        column: x => x.GuideMetaGuid,
                        principalTable: "GuideMetaDatas",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuideMetaGuideDetails_GuideDetailGuid",
                table: "GuideMetaGuideDetails",
                column: "GuideDetailGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuideMetaGuideDetails");

            migrationBuilder.DropTable(
                name: "GuideDetails");
        }
    }
}
