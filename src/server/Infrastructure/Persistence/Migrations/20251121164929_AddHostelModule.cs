using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHostelModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hostel");

            migrationBuilder.CreateTable(
                name: "HostelRooms",
                schema: "hostel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BlockName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FloorNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    OccupiedBeds = table.Column<int>(type: "integer", nullable: false),
                    RoomType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "numeric", nullable: true),
                    Facilities = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostelRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomAllocations",
                schema: "hostel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllocationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VacatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "numeric", nullable: true),
                    BedNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AllocatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    VacatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAllocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HostelRooms_BlockName_RoomNumber",
                schema: "hostel",
                table: "HostelRooms",
                columns: new[] { "BlockName", "RoomNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_AllocationDate",
                schema: "hostel",
                table: "RoomAllocations",
                column: "AllocationDate");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_RoomId",
                schema: "hostel",
                table: "RoomAllocations",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_Status",
                schema: "hostel",
                table: "RoomAllocations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_StudentId",
                schema: "hostel",
                table: "RoomAllocations",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HostelRooms",
                schema: "hostel");

            migrationBuilder.DropTable(
                name: "RoomAllocations",
                schema: "hostel");
        }
    }
}
