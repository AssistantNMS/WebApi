using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NMS.Assistant.Api.Migrations
{
    public partial class ReDoInitialDatabeConstruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Amount = table.Column<string>(nullable: false),
                    DonationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    HashedPassword = table.Column<string>(nullable: false),
                    JoinDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackQuestions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    FeedbackGuid = table.Column<Guid>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackQuestions", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_FeedbackQuestion_Feedback",
                        column: x => x.FeedbackGuid,
                        principalTable: "Feedbacks",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackAnswers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    FeedbackGuid = table.Column<Guid>(nullable: false),
                    FeedbackQuestionGuid = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(nullable: false),
                    AppType = table.Column<int>(nullable: false),
                    AnonymousUserGuid = table.Column<Guid>(nullable: false),
                    DateAnswered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackAnswers", x => x.Guid);
                    table.ForeignKey(
                        name: "ForeignKey_FeedbackAnswer_Feedback",
                        column: x => x.FeedbackGuid,
                        principalTable: "Feedbacks",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ForeignKey_FeedbackAnswer_FeedbackQuestion",
                        column: x => x.FeedbackQuestionGuid,
                        principalTable: "FeedbackQuestions",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswers_FeedbackGuid",
                table: "FeedbackAnswers",
                column: "FeedbackGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswers_FeedbackQuestionGuid",
                table: "FeedbackAnswers",
                column: "FeedbackQuestionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackQuestions_FeedbackGuid",
                table: "FeedbackQuestions",
                column: "FeedbackGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "FeedbackAnswers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FeedbackQuestions");

            migrationBuilder.DropTable(
                name: "Feedbacks");
        }
    }
}
