using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class RemoveVirtualPropertyFromAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVirtual",
                table: "Assets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVirtual",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
