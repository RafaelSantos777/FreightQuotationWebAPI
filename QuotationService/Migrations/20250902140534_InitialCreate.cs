using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuotationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IATACode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CargofiveLocationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialHandlingCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialHandlingCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirQuoteRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginAirportId = table.Column<long>(type: "bigint", nullable: false),
                    DestinationAirportId = table.Column<long>(type: "bigint", nullable: false),
                    LengthCentimeters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    WidthCentimeters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    HeightCentimeters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    WeightKilograms = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SpecialHandlingCodeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQuoteRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQuoteRequests_Locations_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AirQuoteRequests_Locations_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AirQuoteRequests_SpecialHandlingCodes_SpecialHandlingCodeId",
                        column: x => x.SpecialHandlingCodeId,
                        principalTable: "SpecialHandlingCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebCargoRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueCode = table.Column<long>(type: "bigint", nullable: false),
                    Airline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OriginAirportId = table.Column<long>(type: "bigint", nullable: false),
                    DestinationAirportId = table.Column<long>(type: "bigint", nullable: false),
                    VolumetricFactorMetric = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    SpecialHandlingCodeId = table.Column<long>(type: "bigint", nullable: false),
                    MinimumBreakpointCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
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
                name: "AirQuoteResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirQuoteRequestId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQuoteResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQuoteResponses_AirQuoteRequests_AirQuoteRequestId",
                        column: x => x.AirQuoteRequestId,
                        principalTable: "AirQuoteRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebCargoBreakpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    MaximumCost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AirQuotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SpecialHandlingCodeId = table.Column<long>(type: "bigint", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AirQuoteResponseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQuotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQuotes_AirQuoteResponses_AirQuoteResponseId",
                        column: x => x.AirQuoteResponseId,
                        principalTable: "AirQuoteResponses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AirQuotes_SpecialHandlingCodes_SpecialHandlingCodeId",
                        column: x => x.SpecialHandlingCodeId,
                        principalTable: "SpecialHandlingCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirQuoteRequests_DestinationAirportId",
                table: "AirQuoteRequests",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AirQuoteRequests_OriginAirportId",
                table: "AirQuoteRequests",
                column: "OriginAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AirQuoteRequests_SpecialHandlingCodeId",
                table: "AirQuoteRequests",
                column: "SpecialHandlingCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AirQuoteResponses_AirQuoteRequestId",
                table: "AirQuoteResponses",
                column: "AirQuoteRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AirQuotes_AirQuoteResponseId",
                table: "AirQuotes",
                column: "AirQuoteResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_AirQuotes_SpecialHandlingCodeId",
                table: "AirQuotes",
                column: "SpecialHandlingCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CargofiveLocationId",
                table: "Locations",
                column: "CargofiveLocationId",
                unique: true,
                filter: "[CargofiveLocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IATACode",
                table: "Locations",
                column: "IATACode",
                unique: true,
                filter: "[IATACode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialHandlingCodes_Code",
                table: "SpecialHandlingCodes",
                column: "Code",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQuotes");

            migrationBuilder.DropTable(
                name: "WebCargoBreakpoints");

            migrationBuilder.DropTable(
                name: "WebCargoSurcharges");

            migrationBuilder.DropTable(
                name: "AirQuoteResponses");

            migrationBuilder.DropTable(
                name: "WebCargoRates");

            migrationBuilder.DropTable(
                name: "AirQuoteRequests");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SpecialHandlingCodes");
        }
    }
}
