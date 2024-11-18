using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commercial_Office.Migrations
{
    /// <inheritdoc />
    public partial class CreateOfficetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Identificator = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Identificator);
                });

            migrationBuilder.CreateTable(
                name: "AttentionPlaces",
                columns: table => new
                {
                    PlaceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    place_number = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    OfficeIdentificator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttentionPlaces", x => x.PlaceId);
                    table.ForeignKey(
                        name: "FK_AttentionPlaces_Offices_OfficeIdentificator",
                        column: x => x.OfficeIdentificator,
                        principalTable: "Offices",
                        principalColumn: "Identificator");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttentionPlaces_OfficeIdentificator",
                table: "AttentionPlaces",
                column: "OfficeIdentificator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttentionPlaces");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
