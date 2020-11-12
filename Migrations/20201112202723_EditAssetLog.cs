using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class EditAssetLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetLogs_TaskAssignments_TaskAssignmentId",
                table: "AssetLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetLogs_TaskAssignmentId",
                table: "AssetLogs");

            migrationBuilder.DropColumn(
                name: "TaskAssignmentId",
                table: "AssetLogs");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "AssetLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_AssetId",
                table: "AssetLogs",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetLogs_Assets_AssetId",
                table: "AssetLogs",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetLogs_Assets_AssetId",
                table: "AssetLogs");

            migrationBuilder.DropIndex(
                name: "IX_AssetLogs_AssetId",
                table: "AssetLogs");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "AssetLogs");

            migrationBuilder.AddColumn<int>(
                name: "TaskAssignmentId",
                table: "AssetLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_TaskAssignmentId",
                table: "AssetLogs",
                column: "TaskAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetLogs_TaskAssignments_TaskAssignmentId",
                table: "AssetLogs",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
