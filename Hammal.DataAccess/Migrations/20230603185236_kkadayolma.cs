using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hammal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class kkadayolma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "SystemUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "SystemUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_ApplicationUserId",
                table: "SystemUsers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_AspNetUsers_ApplicationUserId",
                table: "SystemUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_AspNetUsers_ApplicationUserId",
                table: "SystemUsers");

            migrationBuilder.DropIndex(
                name: "IX_SystemUsers_ApplicationUserId",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "SystemUsers");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "SystemUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
