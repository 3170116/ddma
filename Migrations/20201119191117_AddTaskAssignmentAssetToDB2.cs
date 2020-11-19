using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class AddTaskAssignmentAssetToDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentAssets_TaskAssignments_TaskAssignmentId",
                table: "TaskAssignmentAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentAssets_Users_UserId",
                table: "TaskAssignmentAssets");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignmentAssets_UserId",
                table: "TaskAssignmentAssets");

            migrationBuilder.AlterColumn<int>(
                name: "TaskAssignmentId",
                table: "TaskAssignmentAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentAssets_AssetId",
                table: "TaskAssignmentAssets",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentAssets_Assets_AssetId",
                table: "TaskAssignmentAssets",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentAssets_TaskAssignments_TaskAssignmentId",
                table: "TaskAssignmentAssets",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentAssets_Assets_AssetId",
                table: "TaskAssignmentAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentAssets_TaskAssignments_TaskAssignmentId",
                table: "TaskAssignmentAssets");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignmentAssets_AssetId",
                table: "TaskAssignmentAssets");

            migrationBuilder.AlterColumn<int>(
                name: "TaskAssignmentId",
                table: "TaskAssignmentAssets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentAssets_UserId",
                table: "TaskAssignmentAssets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentAssets_TaskAssignments_TaskAssignmentId",
                table: "TaskAssignmentAssets",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentAssets_Users_UserId",
                table: "TaskAssignmentAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
