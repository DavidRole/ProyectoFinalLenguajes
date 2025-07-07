using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalLenguajes.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnTime",
                table: "OrderMinutes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnTime",
                table: "OrderMinutes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
