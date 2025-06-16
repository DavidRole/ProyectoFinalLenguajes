using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalLenguajes.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Customers");
        }
    }
}
