using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateSystemuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AltCategoryId",
                table: "SystemUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_AltCategoryId",
                table: "SystemUsers",
                column: "AltCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_AltCategories_AltCategoryId",
                table: "SystemUsers",
                column: "AltCategoryId",
                principalTable: "AltCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_AltCategories_AltCategoryId",
                table: "SystemUsers");

            migrationBuilder.DropIndex(
                name: "IX_SystemUsers_AltCategoryId",
                table: "SystemUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AltCategoryId",
                table: "SystemUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
