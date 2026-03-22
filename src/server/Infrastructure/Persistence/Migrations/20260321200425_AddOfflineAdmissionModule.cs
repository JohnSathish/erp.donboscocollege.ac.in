using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOfflineAdmissionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdmissionChannel",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OfflineIssuedMajorSubject",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfflineFormReceivedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SelectionListPublishedAtUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectionListRound",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectionListRound",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "SelectionListPublishedAtUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "OfflineFormReceivedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "OfflineIssuedMajorSubject",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "AdmissionChannel",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
