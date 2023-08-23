using Microsoft.EntityFrameworkCore.Migrations;

namespace Hubtel.ECommerce.API.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Item_ItemId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_User_UserId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartEntry_Cart_CartId",
                table: "CartEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_CartEntry_Item_ItemId",
                table: "CartEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartEntry",
                table: "CartEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartEntry");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Item",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "CartEntry",
                newName: "CartEntries");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_CartEntry_ItemId",
                table: "CartEntries",
                newName: "IX_CartEntries_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartEntry_CartId",
                table: "CartEntries",
                newName: "IX_CartEntries_CartId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserId",
                table: "Carts",
                newName: "IX_Carts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_ItemId",
                table: "Carts",
                newName: "IX_Carts_ItemId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Items",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartEntries",
                table: "CartEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartEntries_Carts_CartId",
                table: "CartEntries",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartEntries_Items_ItemId",
                table: "CartEntries",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Items_ItemId",
                table: "Carts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartEntries_Carts_CartId",
                table: "CartEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_CartEntries_Items_ItemId",
                table: "CartEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Items_ItemId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartEntries",
                table: "CartEntries");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Item");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Cart");

            migrationBuilder.RenameTable(
                name: "CartEntries",
                newName: "CartEntry");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserId",
                table: "Cart",
                newName: "IX_Cart_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ItemId",
                table: "Cart",
                newName: "IX_Cart_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartEntries_ItemId",
                table: "CartEntry",
                newName: "IX_CartEntry_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartEntries_CartId",
                table: "CartEntry",
                newName: "IX_CartEntry_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Item",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "CartEntry",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartEntry",
                table: "CartEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Item_ItemId",
                table: "Cart",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_User_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartEntry_Cart_CartId",
                table: "CartEntry",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartEntry_Item_ItemId",
                table: "CartEntry",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
