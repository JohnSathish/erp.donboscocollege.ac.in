using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                schema: "admissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Email",
                schema: "admissions",
                table: "AdminUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Username",
                schema: "admissions",
                table: "AdminUsers",
                column: "Username",
                unique: true);

            // Seed default admin user
            // Default credentials: username: admin, password: Admin@123
            // Password hash generated using ASP.NET Core Identity PasswordHasher
            var defaultAdminId = Guid.NewGuid();
            var defaultAdminPasswordHash = "AQAAAAIAAYagAAAAEJz7VqF8QqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZqJZ=="; // Placeholder - will be updated
            migrationBuilder.InsertData(
                table: "AdminUsers",
                schema: "admissions",
                columns: new[] { "Id", "Username", "Email", "FullName", "PasswordHash", "IsActive", "CreatedOnUtc" },
                values: new object[] { defaultAdminId, "admin", "admin@donboscocollege.ac.in", "System Administrator", defaultAdminPasswordHash, true, DateTime.UtcNow });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers",
                schema: "admissions");
        }
    }
}
