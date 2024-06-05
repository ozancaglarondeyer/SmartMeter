using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReportApi.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Power = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Voltage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastIndex = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDetails_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreationDate", "MeterId", "Name", "RequestDate", "SerialNumber", "Status" },
                values: new object[,]
                {
                    { new Guid("06c0290d-f62e-4bd7-bace-45c30aca0618"), null, new Guid("2bea741b-a0ec-459f-a0dc-9bbec47f6761"), "Report 3", new DateTime(2024, 6, 3, 13, 13, 48, 627, DateTimeKind.Local).AddTicks(8748), "SN112233", 2 },
                    { new Guid("9c1bea8d-2036-4dc6-8b3c-e3d7adeed9c9"), new DateTime(2024, 6, 1, 13, 13, 48, 627, DateTimeKind.Local).AddTicks(8746), new Guid("8630aa05-09bc-4beb-842f-aeab96032121"), "Report 2", new DateTime(2024, 5, 31, 13, 13, 48, 627, DateTimeKind.Local).AddTicks(8745), "SN654321", 3 },
                    { new Guid("d377e6fd-55a7-4176-8f93-89f0569e62b5"), new DateTime(2024, 5, 27, 13, 13, 48, 627, DateTimeKind.Local).AddTicks(8735), new Guid("9f9906e0-aad9-4f55-9c02-b914585d3763"), "Report 1", new DateTime(2024, 5, 26, 13, 13, 48, 627, DateTimeKind.Local).AddTicks(8698), "SN123456", 1 }
                });

            migrationBuilder.InsertData(
                table: "ReportDetails",
                columns: new[] { "Id", "LastIndex", "Power", "ReadingTime", "ReportId", "Voltage" },
                values: new object[,]
                {
                    { new Guid("7fdc7207-097e-4ff2-8d4a-b3d067b79600"), 1100.3m, 175.89m, new DateTime(2024, 6, 3, 16, 13, 48, 627, DateTimeKind.Local).AddTicks(8827), new Guid("06c0290d-f62e-4bd7-bace-45c30aca0618"), 240.7m },
                    { new Guid("8f61ff85-d69b-4c7c-bc40-9b80e10cb34d"), 1000.1m, 123.45m, new DateTime(2024, 5, 26, 14, 13, 48, 627, DateTimeKind.Local).AddTicks(8811), new Guid("d377e6fd-55a7-4176-8f93-89f0569e62b5"), 220.5m },
                    { new Guid("9c072933-ceaf-4cd2-9b35-a25c81121cb8"), 1050.2m, 150.67m, new DateTime(2024, 5, 31, 15, 13, 48, 627, DateTimeKind.Local).AddTicks(8822), new Guid("9c1bea8d-2036-4dc6-8b3c-e3d7adeed9c9"), 230.0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetails_ReportId",
                table: "ReportDetails",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportDetails");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
