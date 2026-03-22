using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add EnrolledOnUtc column only if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 
                        FROM information_schema.columns 
                        WHERE table_schema = 'admissions' 
                        AND table_name = 'StudentApplicantAccounts' 
                        AND column_name = 'EnrolledOnUtc'
                    ) THEN
                        ALTER TABLE admissions.""StudentApplicantAccounts"" 
                        ADD COLUMN ""EnrolledOnUtc"" timestamp with time zone;
                    END IF;
                END $$;
            ");

            // Add EnrollmentRemarks column only if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 
                        FROM information_schema.columns 
                        WHERE table_schema = 'admissions' 
                        AND table_name = 'StudentApplicantAccounts' 
                        AND column_name = 'EnrollmentRemarks'
                    ) THEN
                        ALTER TABLE admissions.""StudentApplicantAccounts"" 
                        ADD COLUMN ""EnrollmentRemarks"" character varying(1000);
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrolledOnUtc",
                schema: "admissions",
                table: "StudentApplicantAccounts");

            migrationBuilder.DropColumn(
                name: "EnrollmentRemarks",
                schema: "admissions",
                table: "StudentApplicantAccounts");
        }
    }
}
