using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pets_volunteer_volunteer_id",
                table: "pets");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "volunteer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "pets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "fk_pets_volunteer_volunteer_id",
                table: "pets",
                column: "volunteer_id",
                principalTable: "volunteer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pets_volunteer_volunteer_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "volunteer");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "pets");

            migrationBuilder.AddForeignKey(
                name: "fk_pets_volunteer_volunteer_id",
                table: "pets",
                column: "volunteer_id",
                principalTable: "volunteer",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
