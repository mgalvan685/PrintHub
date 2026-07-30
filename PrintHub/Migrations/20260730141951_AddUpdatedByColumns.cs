using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedByColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "ProjectMaterials",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "ProjectFilaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "PrintEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "Printers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "PriceModifiers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "InventoryTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "Filaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Updated_By",
                table: "CostBreakdowns",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "ProjectMaterials");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "ProjectFilaments");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "PrintEvents");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "PriceModifiers");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "CostBreakdowns");
        }
    }
}
