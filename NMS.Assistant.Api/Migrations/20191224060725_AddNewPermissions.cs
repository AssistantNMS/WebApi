using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddNewPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 14);

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                value: 15);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Type",
                keyValue: 15);
        }
    }
}
