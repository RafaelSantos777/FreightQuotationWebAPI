using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCargoService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebCargoRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueCode = table.Column<long>(type: "bigint", nullable: false),
                    Airline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OriginAirportIATACode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DestinationAirportIATACode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VolumetricFactorMetric = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    SpecialHandlingCodeString = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MinimumBreakpointCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebCargoBreakpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    RateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoBreakpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebCargoBreakpoints_WebCargoRates_RateId",
                        column: x => x.RateId,
                        principalTable: "WebCargoRates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebCargoSurcharges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    MaximumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    RateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoSurcharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebCargoSurcharges_WebCargoRates_RateId",
                        column: x => x.RateId,
                        principalTable: "WebCargoRates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoBreakpoints_RateId",
                table: "WebCargoBreakpoints",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_UniqueCode",
                table: "WebCargoRates",
                column: "UniqueCode");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoSurcharges_RateId",
                table: "WebCargoSurcharges",
                column: "RateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebCargoBreakpoints");

            migrationBuilder.DropTable(
                name: "WebCargoSurcharges");

            migrationBuilder.DropTable(
                name: "WebCargoRates");
        }
    }
}
