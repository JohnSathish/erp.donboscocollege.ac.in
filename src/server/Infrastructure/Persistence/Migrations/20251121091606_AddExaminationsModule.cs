using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExaminationsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "examinations");

            migrationBuilder.CreateTable(
                name: "Assessments",
                schema: "examinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicTermId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassSectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: true),
                    TotalWeightage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MaxMarks = table.Column<int>(type: "integer", nullable: false),
                    PassingMarks = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PublishedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PublishedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultSummaries",
                schema: "examinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicTermId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalMarks = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    MaxMarks = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    GPA = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalCredits = table.Column<int>(type: "integer", nullable: false),
                    EarnedCredits = table.Column<int>(type: "integer", nullable: false),
                    CalculatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CalculatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PublishedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PublishedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultSummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentComponents",
                schema: "examinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MaxMarks = table.Column<int>(type: "integer", nullable: false),
                    PassingMarks = table.Column<int>(type: "integer", nullable: false),
                    Weightage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentComponents_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalSchema: "examinations",
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseResults",
                schema: "examinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResultSummaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalMarks = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    MaxMarks = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    GradePoints = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: true),
                    Credits = table.Column<int>(type: "integer", nullable: false),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseResults_ResultSummaries_ResultSummaryId",
                        column: x => x.ResultSummaryId,
                        principalSchema: "examinations",
                        principalTable: "ResultSummaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkEntries",
                schema: "examinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarksObtained = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Grade = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsAbsent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsExempted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EnteredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EnteredBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ApprovedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ModifiedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AssessmentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkEntries_AssessmentComponents_AssessmentComponentId",
                        column: x => x.AssessmentComponentId,
                        principalSchema: "examinations",
                        principalTable: "AssessmentComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarkEntries_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalSchema: "examinations",
                        principalTable: "Assessments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentComponents_AssessmentId",
                schema: "examinations",
                table: "AssessmentComponents",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentComponents_AssessmentId_DisplayOrder",
                schema: "examinations",
                table: "AssessmentComponents",
                columns: new[] { "AssessmentId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_AcademicTermId",
                schema: "examinations",
                table: "Assessments",
                column: "AcademicTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ClassSectionId",
                schema: "examinations",
                table: "Assessments",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CourseId_AcademicTermId_Code",
                schema: "examinations",
                table: "Assessments",
                columns: new[] { "CourseId", "AcademicTermId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_Status",
                schema: "examinations",
                table: "Assessments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CourseResults_CourseId",
                schema: "examinations",
                table: "CourseResults",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseResults_ResultSummaryId",
                schema: "examinations",
                table: "CourseResults",
                column: "ResultSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseResults_ResultSummaryId_CourseId",
                schema: "examinations",
                table: "CourseResults",
                columns: new[] { "ResultSummaryId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_MarkEntries_AssessmentComponentId_StudentId",
                schema: "examinations",
                table: "MarkEntries",
                columns: new[] { "AssessmentComponentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarkEntries_AssessmentId",
                schema: "examinations",
                table: "MarkEntries",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkEntries_Status",
                schema: "examinations",
                table: "MarkEntries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MarkEntries_StudentId",
                schema: "examinations",
                table: "MarkEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_AcademicTermId",
                schema: "examinations",
                table: "ResultSummaries",
                column: "AcademicTermId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_Status",
                schema: "examinations",
                table: "ResultSummaries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_StudentId",
                schema: "examinations",
                table: "ResultSummaries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultSummaries_StudentId_AcademicTermId",
                schema: "examinations",
                table: "ResultSummaries",
                columns: new[] { "StudentId", "AcademicTermId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseResults",
                schema: "examinations");

            migrationBuilder.DropTable(
                name: "MarkEntries",
                schema: "examinations");

            migrationBuilder.DropTable(
                name: "ResultSummaries",
                schema: "examinations");

            migrationBuilder.DropTable(
                name: "AssessmentComponents",
                schema: "examinations");

            migrationBuilder.DropTable(
                name: "Assessments",
                schema: "examinations");
        }
    }
}
