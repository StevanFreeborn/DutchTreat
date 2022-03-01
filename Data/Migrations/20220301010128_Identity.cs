using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutchTreat.Data.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2022, 3, 1, 1, 1, 28, 15, DateTimeKind.Utc).AddTicks(5553));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2022, 3, 1, 0, 35, 37, 402, DateTimeKind.Utc).AddTicks(4050));
        }
    }
}
