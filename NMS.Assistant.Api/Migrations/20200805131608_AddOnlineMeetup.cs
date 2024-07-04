using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddOnlineMeetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineMeetup2020Submissions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    UserImage = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 100, nullable: false),
                    ExternalUrl = table.Column<string>(maxLength: 100, nullable: false),
                    SortRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineMeetup2020Submissions", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineMeetup2020Submissions");
        }
    }
}
