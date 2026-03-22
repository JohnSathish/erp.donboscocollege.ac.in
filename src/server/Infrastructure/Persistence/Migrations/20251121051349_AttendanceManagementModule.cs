using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceManagementModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "attendance");

            migrationBuilder.CreateTable(
                name: "AttendanceDeviceEvents",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonType = table.Column<int>(type: "integer", nullable: true),
                    CardNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EventTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AttendanceRecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProcessingError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RawData = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceDeviceEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceSessions",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ClassSectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    StaffShiftId = table.Column<Guid>(type: "uuid", nullable: true),
                    SessionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    AcademicYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Term = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsMarked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MarkedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MarkedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                schema: "attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MarkedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MarkedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DeviceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_AttendanceSessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "attendance",
                        principalTable: "AttendanceSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDeviceEvents_CardNumber_EventTimestamp",
                schema: "attendance",
                table: "AttendanceDeviceEvents",
                columns: new[] { "CardNumber", "EventTimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDeviceEvents_DeviceId_EventTimestamp",
                schema: "attendance",
                table: "AttendanceDeviceEvents",
                columns: new[] { "DeviceId", "EventTimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDeviceEvents_IsProcessed",
                schema: "attendance",
                table: "AttendanceDeviceEvents",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDeviceEvents_PersonId_EventTimestamp",
                schema: "attendance",
                table: "AttendanceDeviceEvents",
                columns: new[] { "PersonId", "EventTimestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_PersonId_PersonType_MarkedOnUtc",
                schema: "attendance",
                table: "AttendanceRecords",
                columns: new[] { "PersonId", "PersonType", "MarkedOnUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_SessionId_PersonId",
                schema: "attendance",
                table: "AttendanceRecords",
                columns: new[] { "SessionId", "PersonId" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_AcademicYear_Term",
                schema: "attendance",
                table: "AttendanceSessions",
                columns: new[] { "AcademicYear", "Term" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_SessionDate_ClassSectionId_CourseId",
                schema: "attendance",
                table: "AttendanceSessions",
                columns: new[] { "SessionDate", "ClassSectionId", "CourseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceDeviceEvents",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "AttendanceRecords",
                schema: "attendance");

            migrationBuilder.DropTable(
                name: "AttendanceSessions",
                schema: "attendance");
        }
    }
}
