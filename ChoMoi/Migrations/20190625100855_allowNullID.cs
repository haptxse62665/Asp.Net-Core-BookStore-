using Microsoft.EntityFrameworkCore.Migrations;

namespace ChoMoi.Migrations
{
    public partial class allowNullID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookBuy_BookBuyOffileId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookBuy_BookBuyOnlineId",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "BookBuyOnlineId",
                table: "Book",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BookBuyOffileId",
                table: "Book",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookBuy_BookBuyOffileId",
                table: "Book",
                column: "BookBuyOffileId",
                principalTable: "BookBuy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookBuy_BookBuyOnlineId",
                table: "Book",
                column: "BookBuyOnlineId",
                principalTable: "BookBuy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookBuy_BookBuyOffileId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookBuy_BookBuyOnlineId",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "BookBuyOnlineId",
                table: "Book",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookBuyOffileId",
                table: "Book",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookBuy_BookBuyOffileId",
                table: "Book",
                column: "BookBuyOffileId",
                principalTable: "BookBuy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookBuy_BookBuyOnlineId",
                table: "Book",
                column: "BookBuyOnlineId",
                principalTable: "BookBuy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
