using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_task_2.Migrations
{
    /// <inheritdoc />
    public partial class trufground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Bookingkmodel");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Bookingkmodel");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "Bookingkmodel",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Bookingkmodel",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Bookingkmodel",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Bookingkmodel",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
