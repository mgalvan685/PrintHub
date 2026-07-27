using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFilamentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Density",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "Material_Type",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "Texture",
                table: "Filaments");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Filaments",
                newName: "Material");

            migrationBuilder.RenameColumn(
                name: "Cost_Per_Kg",
                table: "Filaments",
                newName: "Weight_Grams");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Filaments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Filaments");

            migrationBuilder.RenameColumn(
                name: "Weight_Grams",
                table: "Filaments",
                newName: "Cost_Per_Kg");

            migrationBuilder.RenameColumn(
                name: "Material",
                table: "Filaments",
                newName: "Type");

            migrationBuilder.AddColumn<decimal>(
                name: "Density",
                table: "Filaments",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material_Type",
                table: "Filaments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Texture",
                table: "Filaments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
