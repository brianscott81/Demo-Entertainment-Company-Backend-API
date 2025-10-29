using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo_Entertainment_Company_Backend_API.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffAndCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameCredits",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStaff",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RideTickets",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameCredits",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsStaff",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RideTickets",
                table: "Users");
        }
    }
}
