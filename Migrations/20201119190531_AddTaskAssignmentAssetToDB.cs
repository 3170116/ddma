using Microsoft.EntityFrameworkCore.Migrations;

namespace ddma.Migrations
{
    public partial class AddTaskAssignmentAssetToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskAssignmentAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    AssetId = table.Column<int>(nullable: false),
                    TaskAssignmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignmentAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAssignmentAssets_TaskAssignments_TaskAssignmentId",
                        column: x => x.TaskAssignmentId,
                        principalTable: "TaskAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskAssignmentAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentAssets_TaskAssignmentId",
                table: "TaskAssignmentAssets",
                column: "TaskAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentAssets_UserId",
                table: "TaskAssignmentAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAssignmentAssets");
        }
    }
}
