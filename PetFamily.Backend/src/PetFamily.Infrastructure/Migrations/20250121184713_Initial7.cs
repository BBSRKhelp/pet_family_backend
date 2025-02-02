using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requisite",
                table: "pets",
                newName: "requisites");

            migrationBuilder.RenameColumn(
                name: "pet_photo",
                table: "pets",
                newName: "pet_photos");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 20);

            migrationBuilder.AlterColumn<int>(
                name: "position",
                table: "pets",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 19);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "pets",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 22);

            migrationBuilder.AlterColumn<Guid>(
                name: "breed_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 21);

            migrationBuilder.AlterColumn<string>(
                name: "requisites",
                table: "pets",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb")
                .Annotation("Relational:ColumnOrder", 17);

            migrationBuilder.AlterColumn<string>(
                name: "pet_photos",
                table: "pets",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb")
                .Annotation("Relational:ColumnOrder", 18);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requisites",
                table: "pets",
                newName: "requisite");

            migrationBuilder.RenameColumn(
                name: "pet_photos",
                table: "pets",
                newName: "pet_photo");

            migrationBuilder.AlterColumn<Guid>(
                name: "species_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 20);

            migrationBuilder.AlterColumn<int>(
                name: "position",
                table: "pets",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 19);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "pets",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .OldAnnotation("Relational:ColumnOrder", 22);

            migrationBuilder.AlterColumn<Guid>(
                name: "breed_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 21);

            migrationBuilder.AlterColumn<string>(
                name: "requisite",
                table: "pets",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb")
                .OldAnnotation("Relational:ColumnOrder", 17);

            migrationBuilder.AlterColumn<string>(
                name: "pet_photo",
                table: "pets",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb")
                .OldAnnotation("Relational:ColumnOrder", 18);
        }
    }
}
