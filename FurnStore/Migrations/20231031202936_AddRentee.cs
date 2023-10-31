using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnStore.Migrations
{
    /// <inheritdoc />
    public partial class AddRentee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rentee",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rentee",
                table: "Product");
        }
    }
}
