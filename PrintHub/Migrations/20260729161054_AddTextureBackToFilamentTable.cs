using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class AddTextureBackToFilamentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Texture",
                table: "Filaments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Texture",
                table: "Filaments");
        }
    }
}
