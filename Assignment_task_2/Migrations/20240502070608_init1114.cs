using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_task_2.Migrations
{
    /// <inheritdoc />
    public partial class init1114 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TurnamentBookings",
                newName: "CustomerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "TurnamentBookings",
                newName: "Name");
        }
    }
}
