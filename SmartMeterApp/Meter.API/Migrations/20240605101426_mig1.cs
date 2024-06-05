using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeterApi.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Voltage = table.Column<double>(type: "float", nullable: false),
                    LastIndex = table.Column<double>(type: "float", nullable: false),
                    Power = table.Column<double>(type: "float", nullable: false),
                    MeterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "SerialNumber" },
                values: new object[,]
                {
                    { new Guid("331a1e6f-3791-4c1c-a84c-abffc7312f8e"), "Meter654321" },
                    { new Guid("65f8db9c-edf2-499e-909a-078da6089e00"), "Meter123456" },
                    { new Guid("c7a55439-34c6-493d-a525-a72409c8e289"), "Meter112233" }
                });

            migrationBuilder.InsertData(
                table: "MeterReadings",
                columns: new[] { "Id", "LastIndex", "MeterId", "Power", "ReadingTime", "Voltage" },
                values: new object[,]
                {
                    { new Guid("161abf92-688a-4fef-9569-ede53741906e"), 1000.1, new Guid("65f8db9c-edf2-499e-909a-078da6089e00"), 123.45, new DateTime(2024, 5, 26, 14, 14, 25, 840, DateTimeKind.Local).AddTicks(1536), 220.5 },
                    { new Guid("c911a9c8-d1b6-473c-8aae-f15fa0b61b71"), 1050.2, new Guid("331a1e6f-3791-4c1c-a84c-abffc7312f8e"), 150.66999999999999, new DateTime(2024, 5, 31, 15, 14, 25, 840, DateTimeKind.Local).AddTicks(1558), 230.0 },
                    { new Guid("ef473ca7-0bd7-48a3-a57c-1bb1d92f6700"), 1100.3, new Guid("c7a55439-34c6-493d-a525-a72409c8e289"), 175.88999999999999, new DateTime(2024, 6, 3, 16, 14, 25, 840, DateTimeKind.Local).AddTicks(1562), 240.69999999999999 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_MeterId",
                table: "MeterReadings",
                column: "MeterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "Meters");
        }
    }
}
