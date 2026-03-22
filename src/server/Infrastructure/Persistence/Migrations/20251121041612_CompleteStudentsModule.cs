using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompleteStudentsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CounselingRecords",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SessionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CounselorName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CounselorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Issues = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Discussion = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    Recommendations = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ActionPlan = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsFollowUpRequired = table.Column<bool>(type: "boolean", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsConfidential = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounselingRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisciplineRecords",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    IncidentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IncidentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ReportedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Witnesses = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    ActionTaken = table.Column<int>(type: "integer", nullable: false),
                    ActionDetails = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ActionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActionTakenBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    ResolvedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplineRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentExits",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExitType = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequestedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsClearanceCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    ClearanceCompletedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClearanceCompletedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentTransfers",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromProgramCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ToProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToProgramCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    FromShift = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ToShift = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    FromSection = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ToSection = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RequestedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTransfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounselingRecords_IsFollowUpRequired",
                schema: "students",
                table: "CounselingRecords",
                column: "IsFollowUpRequired");

            migrationBuilder.CreateIndex(
                name: "IX_CounselingRecords_SessionDate",
                schema: "students",
                table: "CounselingRecords",
                column: "SessionDate");

            migrationBuilder.CreateIndex(
                name: "IX_CounselingRecords_Status",
                schema: "students",
                table: "CounselingRecords",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CounselingRecords_StudentId",
                schema: "students",
                table: "CounselingRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineRecords_IncidentDate",
                schema: "students",
                table: "DisciplineRecords",
                column: "IncidentDate");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineRecords_IsResolved",
                schema: "students",
                table: "DisciplineRecords",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineRecords_Severity",
                schema: "students",
                table: "DisciplineRecords",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineRecords_StudentId",
                schema: "students",
                table: "DisciplineRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExits_RequestedDate",
                schema: "students",
                table: "StudentExits",
                column: "RequestedDate");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExits_Status",
                schema: "students",
                table: "StudentExits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExits_StudentId",
                schema: "students",
                table: "StudentExits",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransfers_RequestedOnUtc",
                schema: "students",
                table: "StudentTransfers",
                column: "RequestedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransfers_Status",
                schema: "students",
                table: "StudentTransfers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTransfers_StudentId",
                schema: "students",
                table: "StudentTransfers",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounselingRecords",
                schema: "students");

            migrationBuilder.DropTable(
                name: "DisciplineRecords",
                schema: "students");

            migrationBuilder.DropTable(
                name: "StudentExits",
                schema: "students");

            migrationBuilder.DropTable(
                name: "StudentTransfers",
                schema: "students");
        }
    }
}
