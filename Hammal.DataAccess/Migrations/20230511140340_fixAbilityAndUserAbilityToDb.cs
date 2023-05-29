using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixAbilityAndUserAbilityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAbilities_Abilities_AbilityId",
                table: "UserAbilities");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.RenameColumn(
                name: "AbilityId",
                table: "UserAbilities",
                newName: "AltCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAbilities_AbilityId",
                table: "UserAbilities",
                newName: "IX_UserAbilities_AltCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAbilities_AltCategories_AltCategoryId",
                table: "UserAbilities",
                column: "AltCategoryId",
                principalTable: "AltCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAbilities_AltCategories_AltCategoryId",
                table: "UserAbilities");

            migrationBuilder.RenameColumn(
                name: "AltCategoryId",
                table: "UserAbilities",
                newName: "AbilityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAbilities_AltCategoryId",
                table: "UserAbilities",
                newName: "IX_UserAbilities_AbilityId");

            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAbilities_Abilities_AbilityId",
                table: "UserAbilities",
                column: "AbilityId",
                principalTable: "Abilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
