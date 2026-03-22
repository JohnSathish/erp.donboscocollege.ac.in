using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToStudentApplicantAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntranceExamInstructions",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntranceExamScheduledOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntranceExamVenue",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusRemarks",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusUpdatedBy",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdatedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntranceExamInstructions",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "EntranceExamScheduledOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "EntranceExamVenue",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "StatusRemarks",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "StatusUpdatedBy",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "StatusUpdatedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
