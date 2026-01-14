using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FixAanvoerderFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_AspNetUsers_aanvoerderNaamId",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_aanvoerderNaamId",
                table: "product");

            migrationBuilder.DropColumn(
                name: "aanvoerderNaamId",
                table: "product");

            migrationBuilder.AlterColumn<string>(
                name: "aanvoerderId",
                table: "product",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_product_aanvoerderId",
                table: "product",
                column: "aanvoerderId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_AspNetUsers_aanvoerderId",
                table: "product",
                column: "aanvoerderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_AspNetUsers_aanvoerderId",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_aanvoerderId",
                table: "product");

            migrationBuilder.AlterColumn<string>(
                name: "aanvoerderId",
                table: "product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "aanvoerderNaamId",
                table: "product",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_aanvoerderNaamId",
                table: "product",
                column: "aanvoerderNaamId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_AspNetUsers_aanvoerderNaamId",
                table: "product",
                column: "aanvoerderNaamId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
