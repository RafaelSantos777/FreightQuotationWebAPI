using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuotationService.Migrations
{
    /// <inheritdoc />
    public partial class WebCargoServicePartition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebCargoBreakpoints");

            migrationBuilder.DropTable(
                name: "WebCargoSurcharges");

            migrationBuilder.DropTable(
                name: "WebCargoRates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebCargoRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationAirportId = table.Column<long>(type: "bigint", nullable: false),
                    OriginAirportId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialHandlingCodeId = table.Column<long>(type: "bigint", nullable: false),
                    Airline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MinimumBreakpointCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UniqueCode = table.Column<long>(type: "bigint", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    VolumetricFactorMetric = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebCargoRates_Locations_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebCargoRates_Locations_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebCargoRates_SpecialHandlingCodes_SpecialHandlingCodeId",
                        column: x => x.SpecialHandlingCodeId,
                        principalTable: "SpecialHandlingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebCargoBreakpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    WebCargoRateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoBreakpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebCargoBreakpoints_WebCargoRates_WebCargoRateId",
                        column: x => x.WebCargoRateId,
                        principalTable: "WebCargoRates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebCargoSurcharges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    MaximumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    MinimumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WebCargoRateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebCargoSurcharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebCargoSurcharges_WebCargoRates_WebCargoRateId",
                        column: x => x.WebCargoRateId,
                        principalTable: "WebCargoRates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoBreakpoints_WebCargoRateId",
                table: "WebCargoBreakpoints",
                column: "WebCargoRateId");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_DestinationAirportId",
                table: "WebCargoRates",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_OriginAirportId",
                table: "WebCargoRates",
                column: "OriginAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_SpecialHandlingCodeId",
                table: "WebCargoRates",
                column: "SpecialHandlingCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_UniqueCode",
                table: "WebCargoRates",
                column: "UniqueCode");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoSurcharges_WebCargoRateId",
                table: "WebCargoSurcharges",
                column: "WebCargoRateId");
        }
    }
}
