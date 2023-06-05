using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class systemUserUpdateToAbilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abilities",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SystemUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abilities",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SystemUsers");
        }
    }
}
