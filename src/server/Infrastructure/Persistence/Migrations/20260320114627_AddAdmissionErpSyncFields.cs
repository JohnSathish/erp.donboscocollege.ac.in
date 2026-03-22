using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionErpSyncFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ErpStudentId",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpSyncLastError",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ErpSyncedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErpStudentId",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "ErpSyncLastError",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "ErpSyncedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
