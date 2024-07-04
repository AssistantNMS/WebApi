using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class AddPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackAnswer_Feedback",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackAnswer_FeedbackQuestion",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackQuestion_Feedback",
                table: "FeedbackQuestions");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(nullable: false),
                    PermissionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => new { x.UserGuid, x.PermissionType });
                    table.ForeignKey(
                        name: "ForeignKey_UserPermissions_Permissions",
                        column: x => x.PermissionType,
                        principalTable: "Permission",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_UserPermissions_Users",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                column: "Type",
                values: new object[]
                {
                    0,
                    1,
                    2,
                    3,
                    4,
                    5,
                    6
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionType",
                table: "UserPermission",
                column: "PermissionType");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackAnswers_Feedbacks",
                table: "FeedbackAnswers",
                column: "FeedbackGuid",
                principalTable: "Feedbacks",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackAnswers_FeedbackQuestions",
                table: "FeedbackAnswers",
                column: "FeedbackQuestionGuid",
                principalTable: "FeedbackQuestions",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackQuestions_Feedbacks",
                table: "FeedbackQuestions",
                column: "FeedbackGuid",
                principalTable: "Feedbacks",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackAnswers_Feedbacks",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackAnswers_FeedbackQuestions",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "ForeignKey_FeedbackQuestions_Feedbacks",
                table: "FeedbackQuestions");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackAnswer_Feedback",
                table: "FeedbackAnswers",
                column: "FeedbackGuid",
                principalTable: "Feedbacks",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackAnswer_FeedbackQuestion",
                table: "FeedbackAnswers",
                column: "FeedbackQuestionGuid",
                principalTable: "FeedbackQuestions",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "ForeignKey_FeedbackQuestion_Feedback",
                table: "FeedbackQuestions",
                column: "FeedbackGuid",
                principalTable: "Feedbacks",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
