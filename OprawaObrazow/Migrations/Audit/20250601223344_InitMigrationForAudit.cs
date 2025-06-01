using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OprawaObrazow.Migrations.Audit
{
    /// <inheritdoc />
    public partial class InitMigrationForAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "oprawa_audit");

            migrationBuilder.CreateTable(
                name: "clients",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("clients_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deliveries",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("deliveries_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "frame_pieces",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("frame_pieces_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "frames",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("frame_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("suppliers_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "oprawa_audit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    record_id = table.Column<int>(type: "int", nullable: false),
                    entity_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clients",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "deliveries",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "frame_pieces",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "frames",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "oprawa_audit");

            migrationBuilder.DropTable(
                name: "users",
                schema: "oprawa_audit");
        }
    }
}
