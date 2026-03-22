using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staff");

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                schema: "staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmployeeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExitDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    EmergencyContactNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Qualifications = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Specialization = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_Email",
                schema: "staff",
                table: "StaffMembers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_EmployeeNumber",
                schema: "staff",
                table: "StaffMembers",
                column: "EmployeeNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffMembers",
                schema: "staff");
        }
    }
}
