using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAltCategory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AltCategories_CategoryId",
                table: "AltCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AltCategories_Categories_CategoryId",
                table: "AltCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltCategories_Categories_CategoryId",
                table: "AltCategories");

            migrationBuilder.DropIndex(
                name: "IX_AltCategories_CategoryId",
                table: "AltCategories");
        }
    }
}
