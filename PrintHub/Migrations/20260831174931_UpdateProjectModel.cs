using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Labor",
                table: "Projects");

            migrationBuilder.AddColumn<decimal>(
                name: "Finishing_Time",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);


            // Remove old string column
            migrationBuilder.DropColumn(
                name: "Print_Time",
                table: "Projects");

            // Add new decimal column
            migrationBuilder.AddColumn<decimal>(
                name: "Print_Time",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);


            migrationBuilder.CreateTable(
                name: "GlobalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Electricity_Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    Labor_Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    Default_Markup = table.Column<decimal>(type: "numeric", nullable: false),
                    Default_Waste_Multiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    Legacy_Id = table.Column<int>(type: "integer", nullable: true),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false),
                    Updated_By = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalSettings");

            migrationBuilder.RenameColumn(
                name: "Finishing_Time",
                table: "Projects",
                newName: "Labor");

            migrationBuilder.AlterColumn<string>(
                name: "Print_Time",
                table: "Projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
