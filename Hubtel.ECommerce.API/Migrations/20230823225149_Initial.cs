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
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CartId = table.Column<int>(type: "integer", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
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
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CartId = table.Column<int>(type: "integer", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Audit_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Audit_Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartEntries_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartEntries_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartEntries_CartId",
                table: "CartEntries",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartEntries_ItemId",
                table: "CartEntries",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ItemId",
                table: "Carts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartEntries");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
