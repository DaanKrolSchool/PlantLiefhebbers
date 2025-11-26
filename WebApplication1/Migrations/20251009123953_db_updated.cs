using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class db_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aanvoerder",
                columns: table => new
                {
                    aanvoerderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    adres = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Wachtwoord = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aanvoerder", x => x.aanvoerderId);
                });

            migrationBuilder.CreateTable(
                name: "veiling",
                columns: table => new
                {
                    veilingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    startPrijs = table.Column<float>(type: "REAL", nullable: false),
                    startDatum = table.Column<string>(type: "TEXT", nullable: false),
                    klokLocatie = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veiling", x => x.veilingId);
                });

            migrationBuilder.CreateTable(
                name: "veilingmeester",
                columns: table => new
                {
                    veilingmeesterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    adres = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Wachtwoord = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veilingmeester", x => x.veilingmeesterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aanvoerder");

            migrationBuilder.DropTable(
                name: "veiling");

            migrationBuilder.DropTable(
                name: "veilingmeester");
        }
    }
}
