using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentFieldsToStudentApplicantAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApplicationSubmitted",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentCompleted",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentCompletedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentOrderId",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApplicationSubmitted",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "IsPaymentCompleted",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "PaymentCompletedOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "PaymentOrderId",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
