using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToeicReadingApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Exam_Subsystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppExams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppExamAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCorrect = table.Column<int>(type: "int", nullable: false),
                    TotalWrong = table.Column<int>(type: "int", nullable: false),
                    TotalSkipped = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamAttempts_AppExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "AppExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppExamParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Directions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamParts_AppExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "AppExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppQuestionGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamPartId = table.Column<int>(type: "int", nullable: false),
                    GroupTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PassageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuestionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppQuestionGroups_AppExamParts_ExamPartId",
                        column: x => x.ExamPartId,
                        principalTable: "AppExamParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    ExamPartId = table.Column<int>(type: "int", nullable: false),
                    QuestionGroupId = table.Column<int>(type: "int", nullable: true),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppQuestions_AppExamParts_ExamPartId",
                        column: x => x.ExamPartId,
                        principalTable: "AppExamParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppQuestions_AppExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "AppExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppQuestions_AppQuestionGroups_QuestionGroupId",
                        column: x => x.QuestionGroupId,
                        principalTable: "AppQuestionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AppExamAttemptDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamAttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    SelectedOption = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CorrectOption = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamAttemptDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamAttemptDetails_AppExamAttempts_ExamAttemptId",
                        column: x => x.ExamAttemptId,
                        principalTable: "AppExamAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppExamAttemptDetails_AppQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "AppQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppExamAttemptDetails_ExamAttemptId",
                table: "AppExamAttemptDetails",
                column: "ExamAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamAttemptDetails_QuestionId",
                table: "AppExamAttemptDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamAttempts_ExamId",
                table: "AppExamAttempts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamParts_ExamId",
                table: "AppExamParts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionGroups_ExamPartId",
                table: "AppQuestionGroups",
                column: "ExamPartId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestions_ExamId",
                table: "AppQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestions_ExamPartId",
                table: "AppQuestions",
                column: "ExamPartId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestions_QuestionGroupId",
                table: "AppQuestions",
                column: "QuestionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppExamAttemptDetails");

            migrationBuilder.DropTable(
                name: "AppExamAttempts");

            migrationBuilder.DropTable(
                name: "AppQuestions");

            migrationBuilder.DropTable(
                name: "AppQuestionGroups");

            migrationBuilder.DropTable(
                name: "AppExamParts");

            migrationBuilder.DropTable(
                name: "AppExams");
        }
    }
}
