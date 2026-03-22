using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClassXiiPercentageToApplicantAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Model already included this column in snapshot; DB may be missing it. Idempotent for existing DBs.
            migrationBuilder.Sql(
                """
                ALTER TABLE admissions."StudentApplicantAccounts"
                ADD COLUMN IF NOT EXISTS "ClassXIIPercentage" numeric(5,2) NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE admissions."StudentApplicantAccounts"
                DROP COLUMN IF EXISTS "ClassXIIPercentage";
                """);
        }
    }
}
