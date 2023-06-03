using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCategoryIdToSystemUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SystemUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SystemUsers");
        }
    }
}
