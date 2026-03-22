using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectsMasterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects_master",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    StreamCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects_master", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subjects_master_BoardCode_StreamCode_IsActive",
                schema: "admissions",
                table: "subjects_master",
                columns: new[] { "BoardCode", "StreamCode", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_subjects_master_BoardCode_StreamCode_SubjectName",
                schema: "admissions",
                table: "subjects_master",
                columns: new[] { "BoardCode", "StreamCode", "SubjectName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subjects_master",
                schema: "admissions");
        }
    }
}
