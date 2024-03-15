using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitsInStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitInStock",
                table: "Products",
                newName: "UnitsInStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitsInStock",
                table: "Products",
                newName: "UnitInStock");
        }
    }
}
