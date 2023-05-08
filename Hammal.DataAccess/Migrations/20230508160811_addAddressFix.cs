using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAddressFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Addresses");
        }
    }
}
