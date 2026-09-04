using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SubCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    SubCourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCourses_MasterCourses_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    SubCourseId = table.Column<int>(type: "int", nullable: false),
                    TopicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddTopics_MasterCourses_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddTopics_SubCourses_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AddMaterials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    SubCourseId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignmentAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ1Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ2Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCQ3Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddMaterials", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_AddMaterials_AddTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "AddTopics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddMaterials_MasterCourses_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddMaterials_SubCourses_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddMaterials_MasterCourseId",
                table: "AddMaterials",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AddMaterials_SubCourseId",
                table: "AddMaterials",
                column: "SubCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AddMaterials_TopicId",
                table: "AddMaterials",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_AddTopics_MasterCourseId",
                table: "AddTopics",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AddTopics_SubCourseId",
                table: "AddTopics",
                column: "SubCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCourses_MasterCourseId",
                table: "SubCourses",
                column: "MasterCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddMaterials");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AddTopics");

            migrationBuilder.DropTable(
                name: "SubCourses");

            migrationBuilder.DropTable(
                name: "MasterCourses");
        }
    }
}
