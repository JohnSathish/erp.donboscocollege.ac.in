using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicantAuthEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                schema: "admissions",
                table: "StudentApplicantAccounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApplicantRefreshTokens",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantRefreshTokens_StudentApplicantAccounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "admissions",
                        principalTable: "StudentApplicantAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantRefreshTokens_AccountId",
                schema: "admissions",
                table: "ApplicantRefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantRefreshTokens_TokenHash",
                schema: "admissions",
                table: "ApplicantRefreshTokens",
                column: "TokenHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantRefreshTokens",
                schema: "admissions");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
