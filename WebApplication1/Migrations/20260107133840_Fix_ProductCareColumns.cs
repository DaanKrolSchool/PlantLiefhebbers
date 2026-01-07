using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ProductCareColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "makkelijkheid",
                table: "product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "seizoensplant",
                table: "product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "temperatuur",
                table: "product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "water",
                table: "product",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "makkelijkheid",
                table: "product");

            migrationBuilder.DropColumn(
                name: "seizoensplant",
                table: "product");

            migrationBuilder.DropColumn(
                name: "temperatuur",
                table: "product");

            migrationBuilder.DropColumn(
                name: "water",
                table: "product");
        }

    }
}
