using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class AddLogsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLog_TaskAssignments_TaskAssignmentId",
                table: "TaskLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLog_Users_UserId",
                table: "TaskLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLog",
                table: "TaskLog");

            migrationBuilder.DropColumn(
                name: "TaskUserType",
                table: "TaskLog");

            migrationBuilder.RenameTable(
                name: "TaskLog",
                newName: "TaskLogs");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLog_UserId",
                table: "TaskLogs",
                newName: "IX_TaskLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLog_TaskAssignmentId",
                table: "TaskLogs",
                newName: "IX_TaskLogs_TaskAssignmentId");

            migrationBuilder.AddColumn<int>(
                name: "TaskLogType",
                table: "TaskLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLogs",
                table: "TaskLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssetLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskAssignmentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AssetLogType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetLogs_TaskAssignments_TaskAssignmentId",
                        column: x => x.TaskAssignmentId,
                        principalTable: "TaskAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_TaskAssignmentId",
                table: "AssetLogs",
                column: "TaskAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_UserId",
                table: "AssetLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_TaskAssignments_TaskAssignmentId",
                table: "TaskLogs",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Users_UserId",
                table: "TaskLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_TaskAssignments_TaskAssignmentId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Users_UserId",
                table: "TaskLogs");

            migrationBuilder.DropTable(
                name: "AssetLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLogs",
                table: "TaskLogs");

            migrationBuilder.DropColumn(
                name: "TaskLogType",
                table: "TaskLogs");

            migrationBuilder.RenameTable(
                name: "TaskLogs",
                newName: "TaskLog");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLogs_UserId",
                table: "TaskLog",
                newName: "IX_TaskLog_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLogs_TaskAssignmentId",
                table: "TaskLog",
                newName: "IX_TaskLog_TaskAssignmentId");

            migrationBuilder.AddColumn<int>(
                name: "TaskUserType",
                table: "TaskLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLog",
                table: "TaskLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLog_TaskAssignments_TaskAssignmentId",
                table: "TaskLog",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLog_Users_UserId",
                table: "TaskLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
