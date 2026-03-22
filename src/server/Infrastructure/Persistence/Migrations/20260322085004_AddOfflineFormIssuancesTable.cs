using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOfflineFormIssuancesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfflineFormIssuances",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    StudentName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ApplicationFeeAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IssuedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApplicantAccountId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineFormIssuances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfflineFormIssuances_ApplicantAccountId",
                schema: "admissions",
                table: "OfflineFormIssuances",
                column: "ApplicantAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineFormIssuances_FormNumber",
                schema: "admissions",
                table: "OfflineFormIssuances",
                column: "FormNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfflineFormIssuances",
                schema: "admissions");
        }
    }
}
