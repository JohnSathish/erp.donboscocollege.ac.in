using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicHistoryEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicRecords",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcademicYear = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Semester = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GPA = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    CGPA = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    Grade = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    ResultStatus = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    TotalCredits = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreditsEarned = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcademicRecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    EnrollmentType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    EnrolledOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Grade = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    MarksObtained = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    MaxMarks = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    ResultStatus = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CompletedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId",
                schema: "students",
                table: "AcademicRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId_AcademicYear_Semester",
                schema: "students",
                table: "AcademicRecords",
                columns: new[] { "StudentId", "AcademicYear", "Semester" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_TermId",
                schema: "students",
                table: "AcademicRecords",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_AcademicRecordId",
                schema: "students",
                table: "CourseEnrollments",
                column: "AcademicRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId",
                schema: "students",
                table: "CourseEnrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StudentId",
                schema: "students",
                table: "CourseEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StudentId_CourseId_TermId",
                schema: "students",
                table: "CourseEnrollments",
                columns: new[] { "StudentId", "CourseId", "TermId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecords",
                schema: "students");

            migrationBuilder.DropTable(
                name: "CourseEnrollments",
                schema: "students");
        }
    }
}
