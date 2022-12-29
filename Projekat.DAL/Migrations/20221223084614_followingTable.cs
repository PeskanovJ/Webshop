using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekat.DAL.Migrations
{
    public partial class followingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_UserId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UserId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "Followings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Followings_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Followings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followings_ItemId",
                table: "Followings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_UserId",
                table: "Followings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followings");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserId1",
                table: "Items",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_UserId1",
                table: "Items",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
