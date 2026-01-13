using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class voegPrijsToe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aanvoerder");

            migrationBuilder.DropTable(
                name: "klant");

            migrationBuilder.DropTable(
                name: "veilingmeester");

            migrationBuilder.AddColumn<float>(
                name: "prijs",
                table: "product",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prijs",
                table: "product");

            migrationBuilder.CreateTable(
                name: "aanvoerder",
                columns: table => new
                {
                    aanvoerderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aanvoerder", x => x.aanvoerderId);
                });

            migrationBuilder.CreateTable(
                name: "klant",
                columns: table => new
                {
                    klantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_klant", x => x.klantId);
                });

            migrationBuilder.CreateTable(
                name: "veilingmeester",
                columns: table => new
                {
                    veilingmeesterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veilingmeester", x => x.veilingmeesterId);
                });
        }
    }
}
