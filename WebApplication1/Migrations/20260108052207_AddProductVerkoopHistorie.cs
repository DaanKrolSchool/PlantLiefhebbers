using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVerkoopHistorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "productVerkoopHistorie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    soortPlant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aanvoerderNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aantalVerkocht = table.Column<int>(type: "int", nullable: false),
                    prijsPerStuk = table.Column<float>(type: "real", nullable: false),
                    datum = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productVerkoopHistorie", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productVerkoopHistorie");
        }
    }
}
