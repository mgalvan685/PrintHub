using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectPrinterForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Printers_PrinterId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PrinterId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PrinterId",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Printer_ID",
                table: "Projects",
                column: "Printer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Printers_Printer_ID",
                table: "Projects",
                column: "Printer_ID",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Printers_Printer_ID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Printer_ID",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "PrinterId",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PrinterId",
                table: "Projects",
                column: "PrinterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Printers_PrinterId",
                table: "Projects",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
