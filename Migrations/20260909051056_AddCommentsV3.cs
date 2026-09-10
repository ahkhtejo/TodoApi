using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDos.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments");

            migrationBuilder.CreateIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments",
                column: "TodoTaskID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments");

            migrationBuilder.CreateIndex(
                name: "IX_comments_TodoTaskID",
                table: "comments",
                column: "TodoTaskID");
        }
    }
}
