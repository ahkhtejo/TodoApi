using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDos.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueStatusName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Status_StatusName",
                table: "Status",
                column: "StatusName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Status_StatusName",
                table: "Status");
        }
    }
}
