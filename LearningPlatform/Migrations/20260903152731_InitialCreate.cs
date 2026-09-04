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
                name: "MasterCourse",
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
                    table.PrimaryKey("PK_MasterCourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SubCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    SubCourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCourse_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_AddTopics_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddTopics_SubCourse_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    SubCourseId = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    Validity = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.sid);
                    table.ForeignKey(
                        name: "FK_Subscriptions_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubCourse_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CertificateFile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_Certificates_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
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
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddMaterials", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_AddMaterials_AddTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "AddTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddMaterials_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddMaterials_SubCourse_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseProgress",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MasterCourseId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    McqCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProgress", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_CourseProgress_AddTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "AddTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProgress_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MasterCourseId = table.Column<int>(type: "int", nullable: true),
                    SubCourseId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_MasterCourse_MasterCourseId",
                        column: x => x.MasterCourseId,
                        principalTable: "MasterCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_SubCourse_SubCourseId",
                        column: x => x.SubCourseId,
                        principalTable: "SubCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    SolutionFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_AddMaterials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "AddMaterials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AssignmentSubmissions_MaterialId",
                table: "AssignmentSubmissions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_UserId",
                table: "AssignmentSubmissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MasterCourseId",
                table: "Certificates",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_UserId",
                table: "Certificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_MasterCourseId",
                table: "CourseProgress",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_TopicId",
                table: "CourseProgress",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_UserId",
                table: "CourseProgress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_MasterCourseId",
                table: "Purchases",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SubCourseId",
                table: "Purchases",
                column: "SubCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SubscriptionId",
                table: "Purchases",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCourse_MasterCourseId",
                table: "SubCourse",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MasterCourseId",
                table: "Subscriptions",
                column: "MasterCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubCourseId",
                table: "Subscriptions",
                column: "SubCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "CourseProgress");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "AddMaterials");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AddTopics");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SubCourse");

            migrationBuilder.DropTable(
                name: "MasterCourse");
        }
    }
}
