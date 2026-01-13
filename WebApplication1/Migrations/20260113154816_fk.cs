using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aanvoerderNaam",
                table: "productVerkoopHistorie");

            migrationBuilder.DropColumn(
                name: "datum",
                table: "productVerkoopHistorie");

            migrationBuilder.DropColumn(
                name: "soortPlant",
                table: "productVerkoopHistorie");

            migrationBuilder.DropColumn(
                name: "aanvoerderNaam",
                table: "product");

            migrationBuilder.DropColumn(
                name: "adres",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "klantId",
                table: "productVerkoopHistorie",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "veilDatum",
                table: "product",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aanvoerderNaamId",
                table: "product",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_productVerkoopHistorie_klantId",
                table: "productVerkoopHistorie",
                column: "klantId");

            migrationBuilder.CreateIndex(
                name: "IX_productVerkoopHistorie_productId",
                table: "productVerkoopHistorie",
                column: "productId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_productVerkoopHistorie_AspNetUsers_klantId",
                table: "productVerkoopHistorie",
                column: "klantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productVerkoopHistorie_product_productId",
                table: "productVerkoopHistorie",
                column: "productId",
                principalTable: "product",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_AspNetUsers_aanvoerderNaamId",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_productVerkoopHistorie_AspNetUsers_klantId",
                table: "productVerkoopHistorie");

            migrationBuilder.DropForeignKey(
                name: "FK_productVerkoopHistorie_product_productId",
                table: "productVerkoopHistorie");

            migrationBuilder.DropIndex(
                name: "IX_productVerkoopHistorie_klantId",
                table: "productVerkoopHistorie");

            migrationBuilder.DropIndex(
                name: "IX_productVerkoopHistorie_productId",
                table: "productVerkoopHistorie");

            migrationBuilder.DropIndex(
                name: "IX_product_aanvoerderNaamId",
                table: "product");

            migrationBuilder.DropColumn(
                name: "klantId",
                table: "productVerkoopHistorie");

            migrationBuilder.DropColumn(
                name: "aanvoerderNaamId",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "aanvoerderNaam",
                table: "productVerkoopHistorie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "datum",
                table: "productVerkoopHistorie",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "soortPlant",
                table: "productVerkoopHistorie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "veilDatum",
                table: "product",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aanvoerderNaam",
                table: "product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "adres",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
