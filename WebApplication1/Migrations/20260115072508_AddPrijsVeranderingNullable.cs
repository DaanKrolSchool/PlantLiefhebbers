using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddPrijsVeranderingNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // prijsVerandering → nullable
            migrationBuilder.AlterColumn<float>(
                name: "prijsVerandering",
                table: "product",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            // seizoensplant → required
            migrationBuilder.AlterColumn<string>(
                name: "seizoensplant",
                table: "product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // leeftijd → required
            migrationBuilder.AlterColumn<int>(
                name: "leeftijd",
                table: "product",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // veilDatum → required
            migrationBuilder.AlterColumn<DateOnly>(
                name: "veilDatum",
                table: "product",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
