using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutchTreat.Data.Migrations
{
    public partial class SeedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "OrderNumber" },
                values: new object[] { 1, new DateTime(2022, 2, 28, 15, 52, 31, 182, DateTimeKind.Utc).AddTicks(2755), "12345" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
