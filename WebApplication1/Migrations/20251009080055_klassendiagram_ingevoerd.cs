using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class klassendiagram_ingevoerd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "beschrijving",
                table: "product");

            migrationBuilder.RenameColumn(
                name: "prijs",
                table: "product",
                newName: "soortPlant");

            migrationBuilder.AddColumn<int>(
                name: "potMaat",
                table: "product",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "steelLengte",
                table: "product",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "adres",
                table: "klant",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "klant",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Wachtwoord",
                table: "klant",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "potMaat",
                table: "product");

            migrationBuilder.DropColumn(
                name: "steelLengte",
                table: "product");

            migrationBuilder.DropColumn(
                name: "adres",
                table: "klant");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "klant");

            migrationBuilder.DropColumn(
                name: "Wachtwoord",
                table: "klant");

            migrationBuilder.RenameColumn(
                name: "soortPlant",
                table: "product",
                newName: "prijs");

            migrationBuilder.AddColumn<string>(
                name: "beschrijving",
                table: "product",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
