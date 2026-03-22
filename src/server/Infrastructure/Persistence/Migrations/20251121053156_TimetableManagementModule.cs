using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TimetableManagementModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academics");

            migrationBuilder.CreateTable(
                name: "AcademicTerms",
                schema: "academics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TermName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AcademicYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassSections",
                schema: "academics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SectionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeacherName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    EnrolledCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    RoomNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Building = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AcademicYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Shift = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "academics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Building = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Floor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    HasProjector = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HasComputerLab = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HasWhiteboard = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Equipment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimetableSlots",
                schema: "academics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Building = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeacherName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableSlots_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalSchema: "academics",
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicTerms_AcademicYear_TermName",
                schema: "academics",
                table: "AcademicTerms",
                columns: new[] { "AcademicYear", "TermName" });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicTerms_IsActive",
                schema: "academics",
                table: "AcademicTerms",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_CourseId_AcademicYear_Shift",
                schema: "academics",
                table: "ClassSections",
                columns: new[] { "CourseId", "AcademicYear", "Shift" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_TeacherId",
                schema: "academics",
                table: "ClassSections",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber_Building",
                schema: "academics",
                table: "Rooms",
                columns: new[] { "RoomNumber", "Building" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Type",
                schema: "academics",
                table: "Rooms",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableSlots_ClassSectionId_DayOfWeek_StartTime",
                schema: "academics",
                table: "TimetableSlots",
                columns: new[] { "ClassSectionId", "DayOfWeek", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableSlots_RoomNumber_DayOfWeek_StartTime",
                schema: "academics",
                table: "TimetableSlots",
                columns: new[] { "RoomNumber", "DayOfWeek", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableSlots_TeacherId_DayOfWeek_StartTime",
                schema: "academics",
                table: "TimetableSlots",
                columns: new[] { "TeacherId", "DayOfWeek", "StartTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicTerms",
                schema: "academics");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "academics");

            migrationBuilder.DropTable(
                name: "TimetableSlots",
                schema: "academics");

            migrationBuilder.DropTable(
                name: "ClassSections",
                schema: "academics");
        }
    }
}
