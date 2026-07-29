using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class AddLegacyIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "ProjectMaterials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "ProjectFilaments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "PrintEvents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "Printers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "PriceModifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "Materials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "InventoryTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "Filaments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Legacy_Id",
                table: "CostBreakdowns",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "ProjectMaterials");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "ProjectFilaments");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "PrintEvents");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "Printers");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "PriceModifiers");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "Legacy_Id",
                table: "CostBreakdowns");
        }
    }
}
