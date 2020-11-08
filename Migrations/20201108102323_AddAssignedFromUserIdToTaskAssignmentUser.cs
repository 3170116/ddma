using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class AddAssignedFromUserIdToTaskAssignmentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedFromUserId",
                table: "TaskAssignmentUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedFromUserId",
                table: "TaskAssignmentUsers");
        }
    }
}
