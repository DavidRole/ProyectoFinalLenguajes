using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalLenguajes.Data.Migrations
{
    /// <inheritdoc />
    public partial class DishChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "Dishes",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Dishes",
                newName: "Precio");
        }
    }
}
