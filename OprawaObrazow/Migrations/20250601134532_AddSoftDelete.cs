using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OprawaObrazow.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "frames",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "frame_pieces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "oprawa",
                table: "clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "frames");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "frame_pieces");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "deliveries");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "oprawa",
                table: "clients");
        }
    }
}
