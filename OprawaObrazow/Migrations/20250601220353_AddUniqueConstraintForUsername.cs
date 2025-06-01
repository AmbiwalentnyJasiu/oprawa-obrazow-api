using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OprawaObrazow.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                schema: "oprawa",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_username",
                schema: "oprawa",
                table: "users");
        }
    }
}
