using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class MoveTimezoneFromCompanyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
