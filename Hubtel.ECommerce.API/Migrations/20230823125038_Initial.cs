using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hubtel.ECommerce.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CartId = table.Column<int>(type: "integer", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cart_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CartId = table.Column<int>(type: "integer", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartEntry_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartEntry_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ItemId",
                table: "Cart",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartEntry_CartId",
                table: "CartEntry",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartEntry_ItemId",
                table: "CartEntry",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartEntry");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
