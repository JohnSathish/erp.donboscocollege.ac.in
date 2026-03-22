using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntranceExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntranceExams",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExamCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExamDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExamStartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ExamEndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Venue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    VenueAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Instructions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    CurrentRegistrations = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntranceExams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamRegistrations",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    HallTicketNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsPresent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    RegisteredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegisteredBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AttendanceMarkedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AttendanceMarkedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ScoreEnteredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ScoreEnteredBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamRegistrations_EntranceExams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "admissions",
                        principalTable: "EntranceExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamRegistrations_StudentApplicantAccounts_ApplicantAccount~",
                        column: x => x.ApplicantAccountId,
                        principalSchema: "admissions",
                        principalTable: "StudentApplicantAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntranceExams_ExamCode",
                schema: "admissions",
                table: "EntranceExams",
                column: "ExamCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntranceExams_ExamDate",
                schema: "admissions",
                table: "EntranceExams",
                column: "ExamDate");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceExams_IsActive",
                schema: "admissions",
                table: "EntranceExams",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRegistrations_ApplicantAccountId",
                schema: "admissions",
                table: "ExamRegistrations",
                column: "ApplicantAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRegistrations_ExamId",
                schema: "admissions",
                table: "ExamRegistrations",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRegistrations_ExamId_ApplicantAccountId",
                schema: "admissions",
                table: "ExamRegistrations",
                columns: new[] { "ExamId", "ApplicantAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamRegistrations_HallTicketNumber",
                schema: "admissions",
                table: "ExamRegistrations",
                column: "HallTicketNumber",
                unique: true,
                filter: "\"HallTicketNumber\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamRegistrations",
                schema: "admissions");

            migrationBuilder.DropTable(
                name: "EntranceExams",
                schema: "admissions");
        }
    }
}
