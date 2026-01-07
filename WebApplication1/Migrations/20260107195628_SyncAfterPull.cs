using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class SyncAfterPull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "maximumPrijs",
                table: "product",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "isVerkocht",
                table: "product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "verkoopDatum",
                table: "product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "verkoopPrijs",
                table: "product",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isVerkocht",
                table: "product");

            migrationBuilder.DropColumn(
                name: "verkoopDatum",
                table: "product");

            migrationBuilder.DropColumn(
                name: "verkoopPrijs",
                table: "product");

            migrationBuilder.AlterColumn<float>(
                name: "maximumPrijs",
                table: "product",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
