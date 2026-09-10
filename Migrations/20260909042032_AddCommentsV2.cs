using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDos.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TodoTaskID",
                table: "comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments",
                column: "TodoTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Tasks_TodoTaskID",
                table: "comments",
                column: "TodoTaskID",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Tasks_TodoTaskID",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "TodoTaskID",
                table: "comments");
        }
    }
}
