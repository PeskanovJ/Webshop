using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekat.DAL.Migrations
{
    public partial class item_image_foreignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Items_ItemId",
                table: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Items_ItemId",
                table: "Image",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Items_ItemId",
                table: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Items_ItemId",
                table: "Image",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
