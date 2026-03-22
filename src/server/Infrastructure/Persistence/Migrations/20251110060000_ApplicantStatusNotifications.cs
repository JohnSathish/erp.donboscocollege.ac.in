using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicantStatusNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                schema: "admissions",
                table: "Applicants",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "EntranceExamInstructions",
                schema: "admissions",
                table: "Applicants",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntranceExamScheduledOnUtc",
                schema: "admissions",
                table: "Applicants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntranceExamVenue",
                schema: "admissions",
                table: "Applicants",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "admissions",
                table: "Applicants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusRemarks",
                schema: "admissions",
                table: "Applicants",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusUpdatedBy",
                schema: "admissions",
                table: "Applicants",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdatedOnUtc",
                schema: "admissions",
                table: "Applicants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_Status",
                schema: "admissions",
                table: "Applicants",
                column: "Status");

            migrationBuilder.Sql("""
                UPDATE admissions."Applicants"
                SET
                    "MobileNumber" = CASE WHEN COALESCE("MobileNumber", '') = '' THEN 'UNKNOWN' ELSE "MobileNumber" END,
                    "StatusUpdatedOnUtc" = COALESCE("StatusUpdatedOnUtc", "CreatedOnUtc"),
                    "Status" = COALESCE("Status", 0);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applicants_Status",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "EntranceExamInstructions",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "EntranceExamScheduledOnUtc",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "EntranceExamVenue",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "StatusRemarks",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "StatusUpdatedBy",
                schema: "admissions",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "StatusUpdatedOnUtc",
                schema: "admissions",
                table: "Applicants");
        }
    }
}

