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
                name: "Offices",
                columns: table => new
                {
                    OfficeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PositionsAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.OfficeId);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    office = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    place_number = table.Column<long>(type: "bigint", nullable: false),
                    procedure_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    procedure_end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    wait_time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procedures_Offices_office",
                        column: x => x.office,
                        principalTable: "Offices",
                        principalColumn: "OfficeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_office",
                table: "Procedures",
                column: "office");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
