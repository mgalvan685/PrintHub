using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrintHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Texture = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Cost_Per_Kg = table.Column<decimal>(type: "numeric", nullable: false),
                    Material_Type = table.Column<string>(type: "text", nullable: true),
                    Density = table.Column<decimal>(type: "numeric", nullable: true),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Item_Type = table.Column<string>(type: "text", nullable: false),
                    Item_ID = table.Column<int>(type: "integer", nullable: false),
                    Change_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Initial_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Units = table.Column<string>(type: "text", nullable: false),
                    Total_Material = table.Column<decimal>(type: "numeric", nullable: false),
                    Cost_Per_Unit = table.Column<decimal>(type: "numeric", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: true),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Power_Per_Hour = table.Column<decimal>(type: "numeric", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Printer_ID = table.Column<int>(type: "integer", nullable: false),
                    PrinterId = table.Column<int>(type: "integer", nullable: false),
                    Print_Time = table.Column<decimal>(type: "numeric", nullable: false),
                    Labor = table.Column<decimal>(type: "numeric", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Printers_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "Printers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CostBreakdowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Project_ID = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Filament_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Material_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Power_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Labor_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Waste_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Total_Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Calculated_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostBreakdowns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceModifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Project_ID = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Waste_Modifier = table.Column<decimal>(type: "numeric", nullable: false),
                    Power_Usage = table.Column<decimal>(type: "numeric", nullable: false),
                    Profit_Margin = table.Column<decimal>(type: "numeric", nullable: false),
                    Labor_Per_Hour = table.Column<decimal>(type: "numeric", nullable: false),
                    Labor_Time = table.Column<decimal>(type: "numeric", nullable: false),
                    Effective_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: true),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceModifiers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrintEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Project_ID = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Event_Type = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrintEvents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFilaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Project_ID = table.Column<int>(type: "integer", nullable: false),
                    Filament_ID = table.Column<int>(type: "integer", nullable: false),
                    Usage_G = table.Column<decimal>(type: "numeric", nullable: false),
                    Cost_At_Time = table.Column<decimal>(type: "numeric", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFilaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFilaments_Filaments_Filament_ID",
                        column: x => x.Filament_ID,
                        principalTable: "Filaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFilaments_Projects_Project_ID",
                        column: x => x.Project_ID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Project_ID = table.Column<int>(type: "integer", nullable: false),
                    Material_ID = table.Column<int>(type: "integer", nullable: false),
                    Usage = table.Column<decimal>(type: "numeric", nullable: false),
                    Units = table.Column<string>(type: "text", nullable: false),
                    Cost_At_Time = table.Column<decimal>(type: "numeric", nullable: false),
                    Created_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated_On = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created_By = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMaterials_Materials_Material_ID",
                        column: x => x.Material_ID,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterials_Projects_Project_ID",
                        column: x => x.Project_ID,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostBreakdowns_ProjectId",
                table: "CostBreakdowns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceModifiers_ProjectId",
                table: "PriceModifiers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PrintEvents_ProjectId",
                table: "PrintEvents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilaments_Filament_ID",
                table: "ProjectFilaments",
                column: "Filament_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilaments_Project_ID",
                table: "ProjectFilaments",
                column: "Project_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterials_Material_ID",
                table: "ProjectMaterials",
                column: "Material_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterials_Project_ID",
                table: "ProjectMaterials",
                column: "Project_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PrinterId",
                table: "Projects",
                column: "PrinterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostBreakdowns");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "PriceModifiers");

            migrationBuilder.DropTable(
                name: "PrintEvents");

            migrationBuilder.DropTable(
                name: "ProjectFilaments");

            migrationBuilder.DropTable(
                name: "ProjectMaterials");

            migrationBuilder.DropTable(
                name: "Filaments");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Printers");
        }
    }
}
