using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnStore.Migrations
{
    /// <inheritdoc />
    public partial class Added_RenteeEmailProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RenteeEmail",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenteeEmail",
                table: "Product");
        }
    }
}
