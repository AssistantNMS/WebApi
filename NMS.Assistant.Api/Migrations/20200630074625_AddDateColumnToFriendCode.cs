using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddDateColumnToFriendCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PcVisible",
                table: "FriendCode");

            migrationBuilder.DropColumn(
                name: "Ps4Visible",
                table: "FriendCode");

            migrationBuilder.DropColumn(
                name: "Xb1Visible",
                table: "FriendCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSubmitted",
                table: "FriendCode",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSubmitted",
                table: "FriendCode");

            migrationBuilder.AddColumn<bool>(
                name: "PcVisible",
                table: "FriendCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ps4Visible",
                table: "FriendCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Xb1Visible",
                table: "FriendCode",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
