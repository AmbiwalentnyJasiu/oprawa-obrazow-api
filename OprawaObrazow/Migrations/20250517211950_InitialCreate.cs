using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OprawaObrazow.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "oprawa");

            migrationBuilder.CreateTable(
                name: "clients",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    email_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("clients_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    last_supply = table.Column<DateOnly>(type: "date", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    email_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("suppliers_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    date_due = table.Column<DateOnly>(type: "date", nullable: false),
                    is_finished = table.Column<bool>(type: "bit", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    planned_date = table.Column<DateOnly>(type: "date", nullable: true),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    picture_width = table.Column<int>(type: "int", nullable: false),
                    picture_height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pk", x => x.id);
                    table.ForeignKey(
                        name: "orders_clients_id_fk",
                        column: x => x.client_id,
                        principalSchema: "oprawa",
                        principalTable: "clients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "deliveries",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    date_due = table.Column<DateOnly>(type: "date", nullable: false),
                    is_delivered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("deliveries_pk", x => x.id);
                    table.ForeignKey(
                        name: "deliveries_suppliers_fk",
                        column: x => x.supplier_id,
                        principalSchema: "oprawa",
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "frames",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    color = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    has_decoration = table.Column<bool>(type: "bit", nullable: false),
                    storage_location = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("frame_pk", x => x.id);
                    table.ForeignKey(
                        name: "frame_suppliers_id_fk",
                        column: x => x.supplier_id,
                        principalSchema: "oprawa",
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "frame_pieces",
                schema: "oprawa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    length = table.Column<int>(type: "int", nullable: false),
                    is_damaged = table.Column<bool>(type: "bit", nullable: false),
                    frame_id = table.Column<int>(type: "int", nullable: false),
                    storage_location = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    is_in_stock = table.Column<bool>(type: "bit", nullable: false),
                    delivery_id = table.Column<int>(type: "int", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("frame_pieces_pk", x => x.id);
                    table.ForeignKey(
                        name: "frame_pieces_deliveries_fk",
                        column: x => x.delivery_id,
                        principalSchema: "oprawa",
                        principalTable: "deliveries",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "frame_pieces_frame_id_fk",
                        column: x => x.frame_id,
                        principalSchema: "oprawa",
                        principalTable: "frames",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "frame_pieces_orders_id_fk",
                        column: x => x.order_id,
                        principalSchema: "oprawa",
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_supplier_id",
                schema: "oprawa",
                table: "deliveries",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_frame_pieces_delivery_id",
                schema: "oprawa",
                table: "frame_pieces",
                column: "delivery_id");

            migrationBuilder.CreateIndex(
                name: "IX_frame_pieces_frame_id",
                schema: "oprawa",
                table: "frame_pieces",
                column: "frame_id");

            migrationBuilder.CreateIndex(
                name: "IX_frame_pieces_order_id",
                schema: "oprawa",
                table: "frame_pieces",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_frames_supplier_id",
                schema: "oprawa",
                table: "frames",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_client_id",
                schema: "oprawa",
                table: "orders",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "frame_pieces",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "users",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "deliveries",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "frames",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "oprawa");

            migrationBuilder.DropTable(
                name: "clients",
                schema: "oprawa");
        }
    }
}
