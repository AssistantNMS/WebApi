using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddPlatformColumnsForWhatIsNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAndroid",
                table: "WhatIsNews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIos",
                table: "WhatIsNews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeb",
                table: "WhatIsNews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWebApp",
                table: "WhatIsNews",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAndroid",
                table: "WhatIsNews");

            migrationBuilder.DropColumn(
                name: "IsIos",
                table: "WhatIsNews");

            migrationBuilder.DropColumn(
                name: "IsWeb",
                table: "WhatIsNews");

            migrationBuilder.DropColumn(
                name: "IsWebApp",
                table: "WhatIsNews");
        }
    }
}
