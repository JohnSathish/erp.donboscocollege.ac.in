using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccountFieldsToStudentAndGuardian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserAccountId",
                schema: "students",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccountUsername",
                schema: "students",
                table: "Students",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserAccountId",
                schema: "students",
                table: "Guardians",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccountUsername",
                schema: "students",
                table: "Guardians",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAccountId",
                schema: "students",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserAccountUsername",
                schema: "students",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                schema: "students",
                table: "Guardians");

            migrationBuilder.DropColumn(
                name: "UserAccountUsername",
                schema: "students",
                table: "Guardians");
        }
    }
}
