using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class AddCreatedByFKToTaskAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "TaskAssignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_CreatedBy",
                table: "TaskAssignments",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_CreatedBy",
                table: "TaskAssignments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_CreatedBy",
                table: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignments_CreatedBy",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaskAssignments");
        }
    }
}
