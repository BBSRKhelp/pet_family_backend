using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterNameColumnToSpeciesAndToBreeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "species",
                table: "species",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "breed",
                table: "breed",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "species",
                newName: "species");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "breed",
                newName: "breed");
        }
    }
}
