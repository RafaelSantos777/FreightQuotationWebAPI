using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCargoService.Migrations
{
    /// <inheritdoc />
    public partial class Cache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebCargoBreakpoints_WebCargoRates_RateId",
                table: "WebCargoBreakpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_WebCargoSurcharges_WebCargoRates_RateId",
                table: "WebCargoSurcharges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebCargoSurcharges",
                table: "WebCargoSurcharges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebCargoRates",
                table: "WebCargoRates");

            migrationBuilder.DropIndex(
                name: "IX_WebCargoRates_UniqueCode",
                table: "WebCargoRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebCargoBreakpoints",
                table: "WebCargoBreakpoints");

            migrationBuilder.RenameTable(
                name: "WebCargoSurcharges",
                newName: "RateSurcharges");

            migrationBuilder.RenameTable(
                name: "WebCargoRates",
                newName: "Rates");

            migrationBuilder.RenameTable(
                name: "WebCargoBreakpoints",
                newName: "RateBreakpoints");

            migrationBuilder.RenameIndex(
                name: "IX_WebCargoSurcharges_RateId",
                table: "RateSurcharges",
                newName: "IX_RateSurcharges_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_WebCargoBreakpoints_RateId",
                table: "RateBreakpoints",
                newName: "IX_RateBreakpoints_RateId");

            migrationBuilder.AddColumn<string>(
                name: "RateCacheDestinationAirportIATACode",
                table: "Rates",
                type: "nvarchar(3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateCacheOriginAirportIATACode",
                table: "Rates",
                type: "nvarchar(3)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateSurcharges",
                table: "RateSurcharges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateBreakpoints",
                table: "RateBreakpoints",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RateCaches",
                columns: table => new
                {
                    OriginAirportIATACode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DestinationAirportIATACode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateCaches", x => new { x.OriginAirportIATACode, x.DestinationAirportIATACode });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_RateCacheOriginAirportIATACode_RateCacheDestinationAirportIATACode",
                table: "Rates",
                columns: new[] { "RateCacheOriginAirportIATACode", "RateCacheDestinationAirportIATACode" });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_UniqueCode",
                table: "Rates",
                column: "UniqueCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RateBreakpoints_Rates_RateId",
                table: "RateBreakpoints",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_RateCaches_RateCacheOriginAirportIATACode_RateCacheDestinationAirportIATACode",
                table: "Rates",
                columns: new[] { "RateCacheOriginAirportIATACode", "RateCacheDestinationAirportIATACode" },
                principalTable: "RateCaches",
                principalColumns: new[] { "OriginAirportIATACode", "DestinationAirportIATACode" });

            migrationBuilder.AddForeignKey(
                name: "FK_RateSurcharges_Rates_RateId",
                table: "RateSurcharges",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateBreakpoints_Rates_RateId",
                table: "RateBreakpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_RateCaches_RateCacheOriginAirportIATACode_RateCacheDestinationAirportIATACode",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_RateSurcharges_Rates_RateId",
                table: "RateSurcharges");

            migrationBuilder.DropTable(
                name: "RateCaches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateSurcharges",
                table: "RateSurcharges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_RateCacheOriginAirportIATACode_RateCacheDestinationAirportIATACode",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_UniqueCode",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateBreakpoints",
                table: "RateBreakpoints");

            migrationBuilder.DropColumn(
                name: "RateCacheDestinationAirportIATACode",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "RateCacheOriginAirportIATACode",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "RateSurcharges",
                newName: "WebCargoSurcharges");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "WebCargoRates");

            migrationBuilder.RenameTable(
                name: "RateBreakpoints",
                newName: "WebCargoBreakpoints");

            migrationBuilder.RenameIndex(
                name: "IX_RateSurcharges_RateId",
                table: "WebCargoSurcharges",
                newName: "IX_WebCargoSurcharges_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_RateBreakpoints_RateId",
                table: "WebCargoBreakpoints",
                newName: "IX_WebCargoBreakpoints_RateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebCargoSurcharges",
                table: "WebCargoSurcharges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebCargoRates",
                table: "WebCargoRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebCargoBreakpoints",
                table: "WebCargoBreakpoints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WebCargoRates_UniqueCode",
                table: "WebCargoRates",
                column: "UniqueCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WebCargoBreakpoints_WebCargoRates_RateId",
                table: "WebCargoBreakpoints",
                column: "RateId",
                principalTable: "WebCargoRates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WebCargoSurcharges_WebCargoRates_RateId",
                table: "WebCargoSurcharges",
                column: "RateId",
                principalTable: "WebCargoRates",
                principalColumn: "Id");
        }
    }
}
