using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quality_Management.Migrations
{
    /// <inheritdoc />
    public partial class CreateProceduretable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    office = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    place_number = table.Column<long>(type: "bigint", nullable: false),
                    procedure_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    procedure_end = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procedures");
        }
    }
}
