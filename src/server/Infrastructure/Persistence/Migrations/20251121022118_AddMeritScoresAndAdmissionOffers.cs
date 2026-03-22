using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMeritScoresAndAdmissionOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "students");

            migrationBuilder.CreateTable(
                name: "AdmissionOffers",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MeritRank = table.Column<int>(type: "integer", nullable: false),
                    Shift = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MajorSubject = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OfferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AcceptedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeritScores",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ClassXIIPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CuetScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    EntranceExamScore = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    TotalScore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Shift = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MajorSubject = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CalculatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CalculatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeritScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PhotoUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProgramCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    MajorSubject = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    MinorSubject = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Shift = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AcademicYear = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AdmissionNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionOffers_AccountId",
                schema: "admissions",
                table: "AdmissionOffers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionOffers_ApplicationNumber",
                schema: "admissions",
                table: "AdmissionOffers",
                column: "ApplicationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionOffers_Status",
                schema: "admissions",
                table: "AdmissionOffers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MeritScores_AccountId",
                schema: "admissions",
                table: "MeritScores",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeritScores_ApplicationNumber",
                schema: "admissions",
                table: "MeritScores",
                column: "ApplicationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MeritScores_Rank_Shift_MajorSubject",
                schema: "admissions",
                table: "MeritScores",
                columns: new[] { "Rank", "Shift", "MajorSubject" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicYear",
                schema: "students",
                table: "Students",
                column: "AcademicYear");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicantAccountId",
                schema: "students",
                table: "Students",
                column: "ApplicantAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                schema: "students",
                table: "Students",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MobileNumber",
                schema: "students",
                table: "Students",
                column: "MobileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgramId",
                schema: "students",
                table: "Students",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Status",
                schema: "students",
                table: "Students",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                schema: "students",
                table: "Students",
                column: "StudentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmissionOffers",
                schema: "admissions");

            migrationBuilder.DropTable(
                name: "MeritScores",
                schema: "admissions");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "students");
        }
    }
}
