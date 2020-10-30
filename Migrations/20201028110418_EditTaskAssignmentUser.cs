using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class EditTaskAssignmentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentUsers_Users_UserId",
                table: "TaskAssignmentUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TaskAssignmentUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskAssignmentUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentUsers_Users_UserId",
                table: "TaskAssignmentUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentUsers_Users_UserId",
                table: "TaskAssignmentUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskAssignmentUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TaskAssignmentUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentUsers_Users_UserId",
                table: "TaskAssignmentUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
